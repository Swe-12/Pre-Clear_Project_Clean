import { useState, useEffect } from 'react';
import { User, Mail, Shield, Key, Edit, LogOut, Package, MapPin, Phone, Building } from 'lucide-react';
import { getProfile, updateProfile, changePassword } from '../api/auth';

export function ProfilePage({ userRole, onLogout }) {
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [saving, setSaving] = useState(false);
  const [saveMessage, setSaveMessage] = useState('');
  const [passwordMessage, setPasswordMessage] = useState('');
  const [passwordError, setPasswordError] = useState('');

  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    company: '',
    addressLine1: '',
    addressLine2: '',
    city: '',
    state: '',
    postalCode: '',
    countryCode: '',
    timezone: '',
    language: '',
    companyRole: '',
    licenseNumber: '',
    yearsOfExperience: '',
    originCountries: '',
    destinationCountries: '',
    hsCategories: '',
    isAvailable: false,
    maxConcurrentShipments: ''
  });

  const [passwordForm, setPasswordForm] = useState({
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  });

  const timezones = [
    'America/New_York',
    'America/Chicago',
    'America/Denver',
    'America/Los_Angeles',
    'UTC',
    'Europe/London',
    'Europe/Berlin',
    'Asia/Dubai',
    'Asia/Kolkata',
    'Asia/Tokyo'
  ];

  const languages = ['English', 'Spanish', 'French', 'German', 'Chinese', 'Hindi', 'Arabic'];

  const countryCodes = ['US', 'CA', 'MX', 'GB', 'DE', 'FR', 'IN', 'CN', 'AE'];

  useEffect(() => {
    loadProfile();
  }, []);

  const loadProfile = async () => {
    try {
      setLoading(true);
      const profileData = await getProfile();
      setProfile(profileData);
      setFormData(buildForm(profileData));
    } catch (err) {
      console.error('Failed to load profile:', err);
      setError('Failed to load profile data');
    } finally {
      setLoading(false);
    }
  };

  const buildForm = (data) => {
    const role = data?.role?.toLowerCase();
    const shipper = role === 'shipper' ? data.profile || {} : {};
    const broker = role === 'broker' ? data.profile || {} : {};

    return {
      firstName: data?.firstName || '',
      lastName: data?.lastName || '',
      email: data?.email || '',
      phone: data?.phone || '',
      company: data?.company || '',
      addressLine1: shipper.addressLine1 || '',
      addressLine2: shipper.addressLine2 || '',
      city: shipper.city || '',
      state: shipper.state || '',
      postalCode: shipper.postalCode || '',
      countryCode: shipper.countryCode || '',
      timezone: shipper.timezone || broker.timezone || '',
      language: shipper.language || broker.language || '',
      companyRole: shipper.companyRole || '',
      licenseNumber: broker.licenseNumber || '',
      yearsOfExperience: broker.yearsOfExperience ?? '',
      originCountries: (broker.originCountries || []).join(', '),
      destinationCountries: (broker.destinationCountries || []).join(', '),
      hsCategories: (broker.hsCategories || []).join(', '),
      isAvailable: !!broker.isAvailable,
      maxConcurrentShipments: broker.maxConcurrentShipments ?? ''
    };
  };

  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSave = async () => {
    setSaving(true);
    setSaveMessage('');
    try {
      const role = profile?.role?.toLowerCase();
      const payload = {
        firstName: formData.firstName,
        lastName: formData.lastName,
        phone: formData.phone,
        company: formData.company
      };

      if (role === 'shipper') {
        Object.assign(payload, {
          addressLine1: formData.addressLine1,
          addressLine2: formData.addressLine2,
          city: formData.city,
          state: formData.state,
          postalCode: formData.postalCode,
          countryCode: formData.countryCode,
          timezone: formData.timezone,
          language: formData.language,
          companyRole: formData.companyRole
        });
      }

      if (role === 'broker') {
        const toList = (val) => val.split(',').map((v) => v.trim()).filter(Boolean);
        Object.assign(payload, {
          licenseNumber: formData.licenseNumber,
          yearsOfExperience: formData.yearsOfExperience ? Number(formData.yearsOfExperience) : null,
          originCountries: toList(formData.originCountries),
          destinationCountries: toList(formData.destinationCountries),
          hsCategories: toList(formData.hsCategories),
          timezone: formData.timezone,
          language: formData.language,
          isAvailable: formData.isAvailable,
          maxConcurrentShipments: formData.maxConcurrentShipments ? Number(formData.maxConcurrentShipments) : null
        });
      }

      await updateProfile(payload);
      await loadProfile();
      setSaveMessage('Profile updated');
      setEditMode(false);
    } catch (err) {
      console.error('Failed to save profile', err);
      const msg = err?.response?.data?.error || 'Failed to update profile';
      setSaveMessage(msg);
    } finally {
      setSaving(false);
    }
  };

  const handlePasswordChange = async () => {
    setPasswordMessage('');
    setPasswordError('');

    if (!passwordForm.currentPassword || !passwordForm.newPassword) {
      setPasswordError('Enter current and new password');
      return;
    }
    if (passwordForm.newPassword !== passwordForm.confirmPassword) {
      setPasswordError('New passwords do not match');
      return;
    }

    try {
      await changePassword({
        currentPassword: passwordForm.currentPassword,
        newPassword: passwordForm.newPassword
      });
      setPasswordMessage('Password updated');
      setPasswordForm({ currentPassword: '', newPassword: '', confirmPassword: '' });
    } catch (err) {
      console.error('Failed to change password', err);
      const msg = err?.response?.data?.error || 'Failed to change password';
      setPasswordError(msg);
    }
  };

  const getRoleDisplayName = (role) => {
    switch (role?.toLowerCase()) {
      case 'admin': return 'Admin';
      case 'broker': return 'Broker';
      case 'shipper': return 'Shipper';
      default: return 'User';
    }
  };

  const getStatusBadge = (isActive) => {
    return isActive ? (
      <span className="px-3 py-1 bg-green-100 text-green-700 rounded-full text-sm">Active</span>
    ) : (
      <span className="px-3 py-1 bg-red-100 text-red-700 rounded-full text-sm">Inactive</span>
    );
  };

  if (loading) {
    return (
      <div style={{ background: '#FBF9F6', minHeight: '100vh', padding: 24 }}>
        <div className="flex items-center justify-center h-64">
          <div className="text-slate-600">Loading profile...</div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div style={{ background: '#FBF9F6', minHeight: '100vh', padding: 24 }}>
        <div className="flex items-center justify-center h-64">
          <div className="text-red-600">{error}</div>
        </div>
      </div>
    );
  }

  return (
    <div style={{ background: '#FBF9F6', minHeight: '100vh', padding: 24 }}>
      <div className="max-w-4xl mx-auto">
        <div className="mb-8">
          <h1 className="text-slate-900 mb-2" style={{ fontSize: '1.875rem', fontWeight: 600 }}>
            Profile
          </h1>
          <p className="text-slate-600">Manage your account information and settings</p>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          {/* Main Content */}
          <div className="lg:col-span-2 space-y-6">
            {/* Basic Information */}
            <div className="bg-white rounded-xl border border-slate-200 p-6">
              <h2 className="text-lg font-semibold text-slate-900 mb-4 flex items-center gap-2">
                <User className="w-5 h-5" />
                Basic Information
              </h2>
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-slate-700 mb-1">First Name</label>
                  {editMode ? (
                    <input
                      name="firstName"
                      value={formData.firstName}
                      onChange={handleInputChange}
                      className="w-full border rounded-lg px-3 py-2"
                    />
                  ) : (
                    <p className="text-slate-900">{formData.firstName || 'N/A'}</p>
                  )}
                </div>
                <div>
                  <label className="block text-sm font-medium text-slate-700 mb-1">Last Name</label>
                  {editMode ? (
                    <input
                      name="lastName"
                      value={formData.lastName}
                      onChange={handleInputChange}
                      className="w-full border rounded-lg px-3 py-2"
                    />
                  ) : (
                    <p className="text-slate-900">{formData.lastName || 'N/A'}</p>
                  )}
                </div>
                <div>
                  <label className="block text-sm font-medium text-slate-700 mb-1">Email</label>
                  <p className="text-slate-900 flex items-center gap-2">
                    <Mail className="w-4 h-4" />
                    {formData.email || 'N/A'}
                  </p>
                </div>
                <div>
                  <label className="block text-sm font-medium text-slate-700 mb-1">Role</label>
                  <p className="text-slate-900 flex items-center gap-2">
                    <Shield className="w-4 h-4" />
                    {getRoleDisplayName(profile.role)}
                  </p>
                </div>
                <div>
                  <label className="block text-sm font-medium text-slate-700 mb-1">Phone</label>
                  {editMode ? (
                    <input
                      name="phone"
                      value={formData.phone}
                      onChange={handleInputChange}
                      className="w-full border rounded-lg px-3 py-2"
                    />
                  ) : (
                    <p className="text-slate-900 flex items-center gap-2">
                      <Phone className="w-4 h-4" />
                      {formData.phone || 'N/A'}
                    </p>
                  )}
                </div>
                <div>
                  <label className="block text-sm font-medium text-slate-700 mb-1">Company</label>
                  {editMode ? (
                    <input
                      name="company"
                      value={formData.company}
                      onChange={handleInputChange}
                      className="w-full border rounded-lg px-3 py-2"
                    />
                  ) : (
                    <p className="text-slate-900 flex items-center gap-2">
                      <Building className="w-4 h-4" />
                      {formData.company || 'N/A'}
                    </p>
                  )}
                </div>
              </div>
            </div>

            {/* Account Details */}
            <div className="bg-white rounded-xl border border-slate-200 p-6">
              <h2 className="text-lg font-semibold text-slate-900 mb-4">Account Details</h2>
              <div className="space-y-4">
                <div>
                  <label className="block text-sm font-medium text-slate-700 mb-1">User ID</label>
                  <p className="text-slate-900 font-mono">{profile.id || 'N/A'}</p>
                </div>
                <div>
                  <label className="block text-sm font-medium text-slate-700 mb-1">Account Status</label>
                  {getStatusBadge(profile.isActive)}
                </div>
                {saveMessage && <p className="text-sm text-slate-600">{saveMessage}</p>}
              </div>
            </div>

            {/* Role-Specific Information */}
            {profile.role?.toLowerCase() === 'broker' && (
              <div className="bg-white rounded-xl border border-slate-200 p-6">
                <h2 className="text-lg font-semibold text-slate-900 mb-4 flex items-center gap-2">
                  <MapPin className="w-5 h-5" />
                  Broker Configuration
                </h2>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-slate-700 mb-1">License Number</label>
                    {editMode ? (
                      <input
                        name="licenseNumber"
                        value={formData.licenseNumber}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                      />
                    ) : (
                      <p className="text-slate-900">{formData.licenseNumber || 'N/A'}</p>
                    )}
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-slate-700 mb-1">Years Of Experience</label>
                    {editMode ? (
                      <input
                        name="yearsOfExperience"
                        type="number"
                        value={formData.yearsOfExperience}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                      />
                    ) : (
                      <p className="text-slate-900">{formData.yearsOfExperience || 'N/A'}</p>
                    )}
                  </div>
                  <div className="md:col-span-2">
                    <label className="block text-sm font-medium text-slate-700 mb-1">Origin Countries</label>
                    {editMode ? (
                      <input
                        name="originCountries"
                        value={formData.originCountries}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                        placeholder="e.g. US, CA"
                      />
                    ) : (
                      <p className="text-slate-900">{formData.originCountries || 'None'}</p>
                    )}
                  </div>
                  <div className="md:col-span-2">
                    <label className="block text-sm font-medium text-slate-700 mb-1">Destination Countries</label>
                    {editMode ? (
                      <input
                        name="destinationCountries"
                        value={formData.destinationCountries}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                      />
                    ) : (
                      <p className="text-slate-900">{formData.destinationCountries || 'None'}</p>
                    )}
                  </div>
                  <div className="md:col-span-2">
                    <label className="block text-sm font-medium text-slate-700 mb-1">HS Categories</label>
                    {editMode ? (
                      <input
                        name="hsCategories"
                        value={formData.hsCategories}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                        placeholder="e.g. 1001, 1201"
                      />
                    ) : (
                      <p className="text-slate-900">{formData.hsCategories || 'None'}</p>
                    )}
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-slate-700 mb-1">Timezone</label>
                    {editMode ? (
                      <select
                        name="timezone"
                        value={formData.timezone}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                      >
                        <option value="">Select timezone</option>
                        {timezones.map((tz) => (
                          <option key={tz} value={tz}>{tz}</option>
                        ))}
                      </select>
                    ) : (
                      <p className="text-slate-900">{formData.timezone || 'N/A'}</p>
                    )}
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-slate-700 mb-1">Language</label>
                    {editMode ? (
                      <select
                        name="language"
                        value={formData.language}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                      >
                        <option value="">Select language</option>
                        {languages.map((lang) => (
                          <option key={lang} value={lang}>{lang}</option>
                        ))}
                      </select>
                    ) : (
                      <p className="text-slate-900">{formData.language || 'N/A'}</p>
                    )}
                  </div>
                  <div className="flex items-center gap-2">
                    <input
                      type="checkbox"
                      name="isAvailable"
                      checked={formData.isAvailable}
                      onChange={handleInputChange}
                      disabled={!editMode}
                    />
                    <span className="text-sm text-slate-700">Available for new shipments</span>
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-slate-700 mb-1">Max Concurrent Shipments</label>
                    {editMode ? (
                      <input
                        name="maxConcurrentShipments"
                        type="number"
                        value={formData.maxConcurrentShipments}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                      />
                    ) : (
                      <p className="text-slate-900">{formData.maxConcurrentShipments || 'N/A'}</p>
                    )}
                  </div>
                </div>
              </div>
            )}

            {profile.role?.toLowerCase() === 'shipper' && (
              <div className="bg-white rounded-xl border border-slate-200 p-6">
                <h2 className="text-lg font-semibold text-slate-900 mb-4 flex items-center gap-2">
                  <Package className="w-5 h-5" />
                  Shipper Information
                </h2>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <div className="md:col-span-2">
                    <label className="block text-sm font-medium text-slate-700 mb-1">Company Name</label>
                    {editMode ? (
                      <input
                        name="company"
                        value={formData.company}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                      />
                    ) : (
                      <p className="text-slate-900">{formData.company || 'N/A'}</p>
                    )}
                  </div>
                  <div className="md:col-span-2">
                    <label className="block text-sm font-medium text-slate-700 mb-1">Address Line 1</label>
                    {editMode ? (
                      <input
                        name="addressLine1"
                        value={formData.addressLine1}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                      />
                    ) : (
                      <p className="text-slate-900">{formData.addressLine1 || 'N/A'}</p>
                    )}
                  </div>
                  <div className="md:col-span-2">
                    <label className="block text-sm font-medium text-slate-700 mb-1">Address Line 2</label>
                    {editMode ? (
                      <input
                        name="addressLine2"
                        value={formData.addressLine2}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                      />
                    ) : (
                      <p className="text-slate-900">{formData.addressLine2 || 'N/A'}</p>
                    )}
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-slate-700 mb-1">City</label>
                    {editMode ? (
                      <input
                        name="city"
                        value={formData.city}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                      />
                    ) : (
                      <p className="text-slate-900">{formData.city || 'N/A'}</p>
                    )}
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-slate-700 mb-1">State</label>
                    {editMode ? (
                      <input
                        name="state"
                        value={formData.state}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                      />
                    ) : (
                      <p className="text-slate-900">{formData.state || 'N/A'}</p>
                    )}
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-slate-700 mb-1">Postal Code</label>
                    {editMode ? (
                      <input
                        name="postalCode"
                        value={formData.postalCode}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                      />
                    ) : (
                      <p className="text-slate-900">{formData.postalCode || 'N/A'}</p>
                    )}
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-slate-700 mb-1">Country Code</label>
                    {editMode ? (
                      <select
                        name="countryCode"
                        value={formData.countryCode}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                      >
                        <option value="">Select country</option>
                        {countryCodes.map((code) => (
                          <option key={code} value={code}>{code}</option>
                        ))}
                      </select>
                    ) : (
                      <p className="text-slate-900">{formData.countryCode || 'N/A'}</p>
                    )}
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-slate-700 mb-1">Timezone</label>
                    {editMode ? (
                      <select
                        name="timezone"
                        value={formData.timezone}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                      >
                        <option value="">Select timezone</option>
                        {timezones.map((tz) => (
                          <option key={tz} value={tz}>{tz}</option>
                        ))}
                      </select>
                    ) : (
                      <p className="text-slate-900">{formData.timezone || 'N/A'}</p>
                    )}
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-slate-700 mb-1">Language</label>
                    {editMode ? (
                      <select
                        name="language"
                        value={formData.language}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                      >
                        <option value="">Select language</option>
                        {languages.map((lang) => (
                          <option key={lang} value={lang}>{lang}</option>
                        ))}
                      </select>
                    ) : (
                      <p className="text-slate-900">{formData.language || 'N/A'}</p>
                    )}
                  </div>
                  <div className="md:col-span-2">
                    <label className="block text-sm font-medium text-slate-700 mb-1">Company Role</label>
                    {editMode ? (
                      <input
                        name="companyRole"
                        value={formData.companyRole}
                        onChange={handleInputChange}
                        className="w-full border rounded-lg px-3 py-2"
                        placeholder="Owner, Ops Manager"
                      />
                    ) : (
                      <p className="text-slate-900">{formData.companyRole || 'N/A'}</p>
                    )}
                  </div>
                </div>
              </div>
            )}
          </div>

          {/* Sidebar */}
          <div className="space-y-6">
            {/* Security */}
            <div className="bg-white rounded-xl border border-slate-200 p-6">
              <h2 className="text-lg font-semibold text-slate-900 mb-4 flex items-center gap-2">
                <Key className="w-5 h-5" />
                Security
              </h2>
              <div className="space-y-3">
                <input
                  type="password"
                  name="currentPassword"
                  value={passwordForm.currentPassword}
                  onChange={(e) => setPasswordForm({ ...passwordForm, currentPassword: e.target.value })}
                  placeholder="Current password"
                  className="w-full border rounded-lg px-3 py-2"
                />
                <input
                  type="password"
                  name="newPassword"
                  value={passwordForm.newPassword}
                  onChange={(e) => setPasswordForm({ ...passwordForm, newPassword: e.target.value })}
                  placeholder="New password"
                  className="w-full border rounded-lg px-3 py-2"
                />
                <input
                  type="password"
                  name="confirmPassword"
                  value={passwordForm.confirmPassword}
                  onChange={(e) => setPasswordForm({ ...passwordForm, confirmPassword: e.target.value })}
                  placeholder="Confirm new password"
                  className="w-full border rounded-lg px-3 py-2"
                />
                {passwordError && <p className="text-sm text-red-600">{passwordError}</p>}
                {passwordMessage && <p className="text-sm text-green-700">{passwordMessage}</p>}
                <button
                  onClick={handlePasswordChange}
                  className="w-full px-4 py-2 bg-slate-900 text-white rounded-lg hover:bg-slate-800 transition-colors"
                >
                  Change Password
                </button>
              </div>
            </div>

            {/* Actions */}
            <div className="bg-white rounded-xl border border-slate-200 p-6">
              <h2 className="text-lg font-semibold text-slate-900 mb-4">Actions</h2>
              <div className="space-y-3">
                {!editMode ? (
                  <button
                    onClick={() => { setEditMode(true); setSaveMessage(''); }}
                    className="w-full px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors flex items-center gap-2"
                  >
                    <Edit className="w-4 h-4" />
                    Edit Profile
                  </button>
                ) : (
                  <div className="flex gap-3">
                    <button
                      onClick={() => { setEditMode(false); setFormData(buildForm(profile)); }}
                      className="flex-1 px-4 py-2 bg-slate-100 text-slate-700 rounded-lg hover:bg-slate-200 transition-colors"
                    >
                      Cancel
                    </button>
                    <button
                      onClick={handleSave}
                      disabled={saving}
                      className="flex-1 px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors"
                    >
                      {saving ? 'Saving...' : 'Save'}
                    </button>
                  </div>
                )}
                <button
                  onClick={onLogout}
                  className="w-full px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors flex items-center gap-2"
                >
                  <LogOut className="w-4 h-4" />
                  Logout
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}