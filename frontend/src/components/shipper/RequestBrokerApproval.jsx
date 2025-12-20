import { UserCheck, ArrowRight, CheckCircle } from 'lucide-react';
import { shipmentsStore } from '../../store/shipmentsStore';

export function RequestBrokerApproval({ shipment, onNavigate }) {
  const handleSubmit = () => {
    // Update shipment status to awaiting-broker
    const updatedShipment = shipmentsStore.getShipmentById(shipment.id);
    if (updatedShipment) {
      updatedShipment.brokerApproval = 'pending';
      updatedShipment.status = 'awaiting-broker';
      shipmentsStore.saveShipment(updatedShipment);
      
      // Add notification for broker
      shipmentsStore.addNotification({
        id: `notif-${Date.now()}`,
        type: 'broker-approval-request',
        title: 'New Broker Approval Request',
        message: `${updatedShipment.id}: ${updatedShipment.productName} - Ready for review`,
        shipmentId: updatedShipment.id,
        timestamp: new Date().toISOString(),
        read: false,
        recipientRole: 'broker'
      });
    }
    
    onNavigate('dashboard');
  };

  return (
    <div>
      <div className="mb-8">
        <h1 className="text-slate-900 mb-2">Request Broker Approval</h1>
        <p className="text-slate-600">Submit your AI-approved shipment for broker review</p>
      </div>

      <div className="max-w-3xl space-y-6">
        <div className="bg-green-50 rounded-xl p-6 border-2 border-green-200">
          <div className="flex items-center gap-3 mb-4">
            <CheckCircle className="w-8 h-8 text-green-600" />
            <div>
              <h3 className="text-green-900">AI Approval Received</h3>
              <p className="text-green-700 text-sm">Your shipment passed AI compliance checks</p>
            </div>
          </div>
        </div>

        <div className="bg-white rounded-xl p-6 border border-slate-200">
          <h3 className="text-slate-900 mb-4">Next Step: Broker Review</h3>
          <p className="text-slate-600 mb-4">
            A licensed customs broker will review your shipment details and documents to provide final approval.
          </p>
          <ul className="space-y-2 text-slate-600 text-sm">
            <li>• Expected review time: 2-4 hours</li>
            <li>• You'll be notified via email and chat</li>
            <li>• Broker may request additional documents</li>
          </ul>
        </div>

        <button
          onClick={handleSubmit}
          className="w-full px-6 py-4 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition-all shadow-lg flex items-center justify-center gap-2"
        >
          <UserCheck className="w-5 h-5" />
          <span>Submit for Broker Approval</span>
          <ArrowRight className="w-5 h-5" />
        </button>
      </div>
    </div>
  );
}