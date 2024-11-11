namespace ClientManagementApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var clients = await _context.Clients
                .Include(c => c.Addresses)
                .ToListAsync();
            return View(clients);
        }
        public IActionResult Create()
        {
            return View(new Client());
        }

        // POST: Client/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                _context.Update(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }       
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _context.Clients.Include(c => c.Addresses).FirstOrDefaultAsync(c => c.Id == id);
            if (client == null) return NotFound();

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {          
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
