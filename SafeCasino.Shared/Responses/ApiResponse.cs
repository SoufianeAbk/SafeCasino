namespace SafeCasino.Shared.Responses
{
    /// <summary>
    /// Generieke response wrapper voor alle API responses
    /// Biedt een consistente structuur voor succes en fout responses
    /// </summary>
    /// <typeparam name="T">Type van de data die in de response wordt teruggegeven</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Of de API request succesvol was
        /// True = geen fouten, False = er is een fout opgetreden
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// HTTP status code van de response
        /// 200 = OK, 400 = Bad Request, 401 = Unauthorized, 500 = Server Error
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Bericht voor de gebruiker
        /// Bij succes: bevestigingsbericht
        /// Bij fout: beschrijving van wat er mis is gegaan
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// De daadwerkelijke data van de response
        /// Bevat het gewenste object/lijst met gegevens
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Fouten en validatie berichten
        /// Handig voor het weergeven van fouten in formulieren
        /// </summary>
        public Dictionary<string, List<string>> Errors { get; set; } = new();

        /// <summary>
        /// Timestamp van wanneer de response is gegenereerd
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Factory methodes voor het aanmaken van responses

        /// <summary>
        /// Creëert een succesvolle response
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T data, string message = "Request succesvol verwerkt", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creëert een fout response
        /// </summary>
        public static ApiResponse<T> ErrorResponse(string message, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Data = default,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creëert een fout response met validatie fouten
        /// </summary>
        public static ApiResponse<T> ValidationErrorResponse(Dictionary<string, List<string>> errors, string message = "Validatiefouten opgetreden")
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = 400,
                Message = message,
                Data = default,
                Errors = errors,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creëert een "niet gevonden" response (404)
        /// </summary>
        public static ApiResponse<T> NotFoundResponse(string message = "Gevraagde resource niet gevonden")
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = 404,
                Message = message,
                Data = default,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creëert een "unauthorized" response (401)
        /// </summary>
        public static ApiResponse<T> UnauthorizedResponse(string message = "U bent niet geautoriseerd voor deze actie")
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = 401,
                Message = message,
                Data = default,
                Timestamp = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// Niet-generieke versie van ApiResponse voor responses zonder data
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Of de API request succesvol was
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// HTTP status code van de response
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Bericht voor de gebruiker
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Fouten en validatie berichten
        /// </summary>
        public Dictionary<string, List<string>> Errors { get; set; } = new();

        /// <summary>
        /// Timestamp van wanneer de response is gegenereerd
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Factory methodes

        /// <summary>
        /// Creëert een succesvolle response
        /// </summary>
        public static ApiResponse SuccessResponse(string message = "Request succesvol verwerkt", int statusCode = 200)
        {
            return new ApiResponse
            {
                Success = true,
                StatusCode = statusCode,
                Message = message,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creëert een fout response
        /// </summary>
        public static ApiResponse ErrorResponse(string message, int statusCode = 400)
        {
            return new ApiResponse
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}