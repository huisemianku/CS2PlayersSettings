using ClosedXML.Excel;
using CS2PlayersSettings.Data.Repository.Entities;
using CS2PlayersSettings.Data.Repository.Helper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
namespace CS2PlayersSettings.Pages.ExcelImportD
{
    public class ImportDataByExcelModel : PageModel
    {

        private readonly ImportDataByExcel _importDataByExcel;
        public ImportDataByExcelModel(ImportDataByExcel importDataByExcel)
        {
            _importDataByExcel = importDataByExcel;
        }
        [BindProperty]
        public IFormFile ExcelFile { get; set; }
        [BindProperty]
        public IFormFile ExcelTeamFile { get; set; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostUploadExcelAsync()
        {
            return await _importDataByExcel.GetDataByExcel(ExcelFile);
        }
        public async Task<IActionResult> OnPostUploadTeamExcelAsync()
        {
            return await _importDataByExcel.GetTeamDataByExcel(ExcelTeamFile);
        }
    }
}

