using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplicationCRUD.Services;
using WebApplicationCRUD.Models;
using WebApplicationCRUD.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using WebApplicationCRUD.Services.Exceptions;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebApplicationCRUD.Data {
    public class SellersController : Controller {

        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService,
            DepartmentService departmentService) {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index() {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Create() {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SellerFormViewModel SellerFormViewModelObj) {
            if (!ModelState.IsValid) return View(SellerFormViewModelObj);

            await _sellerService.InsertAsync(SellerFormViewModelObj.Seller);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id == null) return RedirectToAction(nameof(Error), new { Message = "Id not provided." });
            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null) return RedirectToAction(nameof(Error), new { Message = "Id not founded." });
            else return View(obj);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id) {
            await _sellerService.RemoveAsync(id);
            try {
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException) {
                return RedirectToAction(nameof(Error), new { Message = "Você não pode excluir esse vendedor porque ele possui vendas" });
            }

        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null) return RedirectToAction(nameof(Error), new { Message = "Id not provided." });
            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null) return RedirectToAction(nameof(Error), new { Message = "Id not founded." });
            else return View(obj);
        }

        public async Task<IActionResult> Edit(int? id) {
            if (id == null) return RedirectToAction(nameof(Error), new { Message = "Id not provided." });
            var seller = await _sellerService.FindByIdAsync(id.Value);
            if (seller == null) return RedirectToAction(nameof(Error), new { Message = "Id not founded." });
            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SellerFormViewModel SellerFormViewModelObj) {
            if (!ModelState.IsValid) return View(SellerFormViewModelObj);
            if (id != SellerFormViewModelObj.Seller.Id) return RedirectToAction(nameof(Error), new { Message = "Id mismatch." });

            try {
                await _sellerService.UpdateAsync(SellerFormViewModelObj.Seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e) {
                return RedirectToAction(nameof(Error), new { Message = e.Message });
            }
        }

        public IActionResult Error(string message) {
            var viewModel = new ErrorViewModel {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}

