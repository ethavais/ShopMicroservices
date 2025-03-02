using Marten;

namespace BuildingBlocks.Database
{
    public class MartenDiagnostics : DocumentSessionListenerBase
    {
        private static readonly TimeSpan SlowTransactionThreshold = TimeSpan.FromMilliseconds(100);
        private readonly System.Diagnostics.Stopwatch _stopwatch = new();
        
        public override void BeforeSaveChanges(IDocumentSession session)
        {
            _stopwatch.Restart();
        }
        
        public void AfterSaveChanges(IDocumentSession session, System.Collections.Generic.IReadOnlyList<System.Exception> exceptions)
        {
            _stopwatch.Stop();
            
            // Only log detailed info for slow transactions
            if (_stopwatch.Elapsed > SlowTransactionThreshold)
            {
                try
                {
                    Console.WriteLine($"Slow transaction detected: {DateTime.Now}, Duration: {_stopwatch.ElapsedMilliseconds}ms");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in session listener: {ex.Message}");
                }
            }
        }

        public void AfterCommitAsync(IDocumentSession session, IReadOnlyList<Exception> exceptions)
        {
            _stopwatch.Stop();
            
            // Only log detailed info for slow transactions
            if (_stopwatch.Elapsed > SlowTransactionThreshold)
            {
                try
                {
                    // Get operation count safely - Removed for simplicity
                    Console.WriteLine($"Slow commit detected: {DateTime.Now}, Duration: {_stopwatch.ElapsedMilliseconds}ms");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in session listener: {ex.Message}");
                }
            }
        }
    }
} 