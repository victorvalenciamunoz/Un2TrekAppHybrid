using ErrorOr;

namespace Un2TrekApp;

internal static class Errors
{
    public static readonly Dictionary<string, Error> ErrorDictionary = new()
    {
        { "A001", Error.Validation(description: "Actividad no encontrada", code: "A001") },
        { "A002", Error.Validation(description: "No hay trekis para esa actividad", code: "A002") },
        { "T001", Error.Validation(description: "No se ha encontrado el treki", code: "T001") },
        { "T003", Error.Validation(description: "El treki no pertenece a la actividad", code: "T003") },
        { "U001", Error.Validation(description: "No se ha encontrado el usaurio", code: "U001") },
        { "U002", Error.Validation(description: "Usuario o contraseña erróneos", code: "U002") },
        { "T002", Error.Validation(code: "T002", description: "Estás muy lejos para capturar este treki") },
        { "T004", Error.Validation(code: "T004", description: "Ya has capturado este treki") }
    };

    public static Error GetErrorByCode(string code)
    {
        return ErrorDictionary.TryGetValue(code, out var error) ? error : Error.Unexpected(description:"General error");
    }
}