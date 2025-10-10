using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ADOLab.Logging
{
    /// <summary>
    /// Logger simples baseado em arquivo para fins de laboratório.
    /// </summary>
    public class FileLogger
    {
        private readonly string _filePath;

        /// <summary>Inicializa um novo logger com o caminho informado.</summary>
        public FileLogger(string filePath) => _filePath = filePath;

        /// <summary>Registra mensagem no arquivo com nível e data/hora.</summary>
        private async Task LogToFileAsync(string level, string message)
        {
            try
            {
                var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}{Environment.NewLine}";
                await File.AppendAllTextAsync(_filePath, line, Encoding.UTF8);
            }
            catch { /* não interrompe a aplicação se falhar o log */ }
        }

        /// <summary>Log de informação.</summary>
        public Task Info(string message) => LogToFileAsync("INFO", message);

        /// <summary>Log de aviso.</summary>
        public Task Warn(string message) => LogToFileAsync("WARN", message);

        /// <summary>Log de erro.</summary>
        public Task Error(string message) => LogToFileAsync("ERROR", message);
    }
}
