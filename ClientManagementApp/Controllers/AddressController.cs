namespace ClientManagementApp.Controllers
{
    public class AddressController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AddressController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Create(int clientId)
        {
            ViewBag.Clients = _context.Clients.ToList();
            var address = new Address { ClientId = clientId };
            return View(address);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Address address)
        {
            var existingAddress = await _context.Addresses
                .Where(a => a.ClientId == address.ClientId && a.Type == address.Type)
                .FirstOrDefaultAsync();

            if (existingAddress != null)
            {              
                ModelState.AddModelError("", $"Az ügyfélnek már van egy {address.Type} címe.");
                ViewBag.Clients = _context.Clients.ToList();
                return View(address); 
            }

            if (ModelState.IsValid)
            {
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Client");
            }

            ViewBag.Clients = _context.Clients.ToList();
            return View(address); 
        }
        public async Task<IActionResult> Edit(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return NotFound();

            ViewBag.Clients = _context.Clients.ToList();
            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Address address)
        {
            if (id != address.Id) return BadRequest();
            var existingAddress = await _context.Addresses
                .Where(a => a.ClientId == address.ClientId && a.Type == address.Type && a.Id != address.Id)
                .FirstOrDefaultAsync();

            if (existingAddress != null)
            {
                ModelState.AddModelError("", $"Az ügyfélnek már van egy {address.Type} címe.");
                ViewBag.Clients = _context.Clients.ToList();
                return View(address);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(address).Property(a => a.ClientId).IsModified = false;
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Addresses.Any(a => a.Id == id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Index", "Client");
            }

            ViewBag.Clients = _context.Clients.ToList();
            return View(address);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var address = await _context.Addresses.Include(a => a.Client).FirstOrDefaultAsync(a => a.Id == id);
            if (address == null) return NotFound();

            return View(address); 
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return NotFound();

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Client"); 
        }

    }
}
