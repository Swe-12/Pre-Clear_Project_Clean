using System.Threading.Tasks;
using PreClear.Api.Data;
using PreClear.Api.Interfaces;
using PreClear.Api.Models;

namespace PreClear.Api.Repositories
{
    public class AiRepository : IAiRepository
    {
        private readonly PreclearDbContext _db;

        public AiRepository(PreclearDbContext db) => _db = db;

        public async Task SaveFindingAsync(AiFinding finding)
        {
            // AiFindings are no longer part of the new schema
            // Compliance information is stored in ShipmentCompliance instead
            await Task.CompletedTask;
        }
    }
}
