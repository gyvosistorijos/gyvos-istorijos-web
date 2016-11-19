using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace Hintme.Models
{
    public class GoogleSheetsRepository : IHintRepository
    {

        private readonly SheetsService _service;

        public GoogleSheetsRepository()
        {
            string certFile = "GoogleCERT.p12";
            string certSecret = "notasecret";
            string serviceAccountEmail = "buoyant-country-126011@appspot.gserviceaccount.com";

            if (!File.Exists(certFile))
            {
                return;
            }

            var certificate = new X509Certificate2(certFile, certSecret,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            var credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(serviceAccountEmail)
                {
                    Scopes = new[] { SheetsService.Scope.Spreadsheets }
                }.FromCertificate(certificate));

            _service = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential
            });
        }

        private Task<int> AppendRow( IEnumerable<string> values)
        {
            var spreadsheetId = "1Kjh3SRNunyuChKuESNWEbhL4SuNSV336FYdoIorzgJk";
            var sheetId = 0;

            var rowValues = values.Select(value =>
                new CellData
                {
                    UserEnteredValue = new ExtendedValue
                    {
                        StringValue = value
                    }
                }).ToList();

            var appendCellRequest = new Request
            {
                AppendCells = new AppendCellsRequest
                {
                    Fields = "*",
                    SheetId = sheetId,
                    Rows = new List<RowData>
                    {
                        new RowData
                        {
                            Values = rowValues
                        }
                    }
                }
            };

            var batchUpdateRequests = new BatchUpdateSpreadsheetRequest
            {
                Requests = new List<Request> { appendCellRequest }
            };

            var batchUpdateTask = _service.Spreadsheets.BatchUpdate(batchUpdateRequests, spreadsheetId).ExecuteAsync();

            return
                batchUpdateTask.ContinueWith(
                    a =>
                    {
                        var count = a.Result.Replies.Count;
                        return count;
                    },
                    TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private IList<IList<object>> GetRows()
        {
            var spreadsheetId = "1Kjh3SRNunyuChKuESNWEbhL4SuNSV336FYdoIorzgJk";
 
            return _service.Spreadsheets.Values.Get(spreadsheetId, "A2:Z9999").Execute().Values;
          
        }

        public IEnumerable<Hint> GetHints()
        {
            var rows = GetRows();
            var hints = rows.Select(x =>
            {
                var values = x.Select(v => (string) v).ToList();
                return new Hint()
                {
                    Header = values.Skip(5).FirstOrDefault(),
                    Id = new Guid(values[0]),
                    Latitude = double.Parse(values[3],CultureInfo.InvariantCulture),
                    Longitude = double.Parse(values[4], CultureInfo.InvariantCulture),
                    Text = values[1],
                    Url = string.IsNullOrEmpty(values[2]) ? null : values[2]
                };
            });

            return hints;
        }

    public void SaveHint(Hint hint)
        {
            var list = new List<string>
            {
                hint.Id.ToString(),
                hint.Text,
                hint.Url,
                hint.Latitude.ToString(CultureInfo.InvariantCulture),
                hint.Longitude.ToString(CultureInfo.InvariantCulture),
                hint.Header
            };

            var a = AppendRow(list).Result;
        }

        public Task<bool> SaveContextAsync()
        {
            return Task.FromResult(true);
        }
    }
}