using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace apiPosto.Filters;

public class ApiExceptionFilter: IExceptionFilter
{
    //Codigo de exceção global para tratar erros não tratados e retornar uma resposta padronizada
    private readonly ILogger<ApiExceptionFilter> _logger;
    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }
    
    
    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Ocorreu uma exceçao não tratada: Status Code 500");

        context.Result = new ObjectResult("Ocorreu um problema ao tratada sua socilitação: Status Code 500")
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };
    }
    
        
}
