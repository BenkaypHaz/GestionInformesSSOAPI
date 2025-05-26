using GestionsInformesSSOAPI.Features.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.WebForms;

namespace GestionsInformesSSOAPI.Features.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("PresupuestoGeneral")]
        public async Task<IActionResult> GetPresupuestoGeneral([FromQuery] int informeId)
        {
            try
            {
                informeId = 26;
                var parameters = new ReportParameter[]
                {       
                new ReportParameter("idInfo", informeId.ToString()),
                new ReportParameter("id_informe", informeId.ToString())  

                };

                var reportBytes = await _reportService.GenerateReportAsync("/Reports_GestionInformesSSO/Informe_Calor", parameters);

                return File(reportBytes, "application/pdf", "report.pdf");
            } 
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }     
        }    
    } 
}
