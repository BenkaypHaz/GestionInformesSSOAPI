using GestionsInformesSSOAPI.Features.Utility;
using System.Collections.Generic;
using System.IO;

public class ExcelService
{
    private readonly ExcelRepository _repository;

    public ExcelService(ExcelRepository repository)
    {
        _repository = repository;
    }

    public ApiResponse LeerArchivoExcel(Stream excelStream, string contentType, int informeId)
    {
        try
        {
            var datosProcesados = _repository.LeerTodasLasHojas(excelStream, contentType);

            // Guardar datos asociados al informeId
            _repository.GuardarDatos(datosProcesados, informeId);

            return ApiResponse.Ok("Archivo procesado y datos guardados exitosamente.");
        }
        catch (Exception ex)
        {
            return ApiResponse.BadRequest($"Error durante el procesamiento del archivo Excel: {ex.Message}");
        }
    }

}
