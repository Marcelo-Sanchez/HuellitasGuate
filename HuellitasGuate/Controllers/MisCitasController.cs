using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HuellitasGuate.Data;
using HuellitasGuate.Models;
using Microsoft.AspNetCore.Identity;
using HuellitasGuate.Areas.Identity.Data;
using Microsoft.Build.Framework;

namespace HuellitasGuate.Controllers
{
    public class MisCitasController : Controller
    {
        private readonly HuellitasGuateContext _context;
        private readonly UserManager <HuellitasGuateUser> _userManager;

        public MisCitasController(HuellitasGuateContext context, UserManager<HuellitasGuateUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: MisCitas
        public async Task<IActionResult> Index()
        {
            // Obtiene el usuario actualmente autenticado
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // Filtra las citas relacionadas con el usuario actual
                var userCitas =  _context.Citas
                    .Where(c => c.Correo == user.Email)
                    .ToList();
                userCitas.ForEach(x => {
                    x.Servicio = _context.Servicios.Single(y => y.Id == x.ServicioId);
                });

                return View(userCitas);
            }
            else
            {
                return Problem("Usuario no autenticado.");
            }
        }


        // GET: MisCitas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Citas == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .FirstOrDefaultAsync(m => m.Id == id);
            cita.Servicio = await _context.Servicios.FindAsync(cita.ServicioId);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // GET: MisCitas/Create
        public IActionResult Create()
        {
            // Cargar la lista de servicios disponibles
            ViewBag.Servicios = new SelectList(_context.Servicios, "Id", "Nombre");
            return View();
            return View();
        }

        // POST: MisCitas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cita cita)
        {
            _context.Add(cita);
            await _context.SaveChangesAsync();
            var email = new EmailSender();
            string html = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Detalles de la Cita</title>\r\n    <style>\r\n        body {{\r\n            font-family: Arial, sans-serif;\r\n            background-color: #f2f2f2;\r\n            margin: 0;\r\n            padding: 0;\r\n        }}\r\n\r\n        .container {{\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            background-color: #ffffff;\r\n            padding: 20px;\r\n            border-radius: 5px;\r\n            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);\r\n        }}\r\n\r\n        h1 {{\r\n            color: #333;\r\n            text-align: center;\r\n        }}\r\n\r\n        .field-label {{\r\n            font-weight: bold;\r\n        }}\r\n\r\n        .field-value {{\r\n            margin-bottom: 20px;\r\n        }}\r\n\r\n        p {{\r\n            margin: 0;\r\n        }}\r\n\r\n        .btn {{\r\n            display: block;\r\n            width: 100%;\r\n            padding: 10px;\r\n            background-color: #007bff;\r\n            color: #fff;\r\n            text-align: center;\r\n            text-decoration: none;\r\n            border: none;\r\n            border-radius: 3px;\r\n            cursor: pointer;\r\n        }}\r\n\r\n        .btn:hover {{\r\n            background-color: #0056b3;\r\n        }}\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <h1>Detalles de la Cita</h1>\r\n        <div class=\"field-value\">\r\n            <p class=\"field-label\">Nombre del Cliente:</p>\r\n            <p>{cita.Nombre}</p>\r\n        </div>\r\n        <div class=\"field-value\">\r\n            <p class=\"field-label\">Fecha de la Cita:</p>\r\n            <p>{cita.Fecha}</p>\r\n        </div>\r\n   \r\n      \r\n        <a href=\"#\" class=\"btn\">Confirmar Cita</a>\r\n    </div>\r\n</body>\r\n</html>\r\n";
            await email.SendEmailAsync(cita.Correo, "Cita Huellitas", html);
            return RedirectToAction(nameof(Index));
        }

        // GET: MisCitas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Citas == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas.FindAsync(id);
            cita.Servicio = await _context.Servicios.FindAsync(cita.ServicioId);
            // Cargar la lista de servicios disponibles
            ViewBag.Servicios = new SelectList(_context.Servicios, "Id", "Nombre");
            if (cita == null)
            {
                return NotFound();
            }
            return View(cita);
        }

        // POST: MisCitas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Mascota,ServicioId,Telefono,Fecha,Dpi,Correo,Descripcion")] Cita cita)
        {
            if (id != cita.Id)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(cita);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaExists(cita.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            return View(cita);
        }

        // GET: MisCitas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Citas == null)
            {
                return NotFound();
            }

            var cita = await _context.Citas
                .FirstOrDefaultAsync(m => m.Id == id);
            cita.Servicio = await _context.Servicios.FindAsync(cita.ServicioId);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        // POST: MisCitas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Citas == null)
            {
                return Problem("Entity set 'HuellitasGuateContext.Citas'  is null.");
            }
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null)
            {
                _context.Citas.Remove(cita);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitaExists(int id)
        {
          return (_context.Citas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
