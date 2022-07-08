//using Implementation3_API.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Implementation3_API.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculationController : ControllerBase
    {
        private readonly ILogger<CalculationController> _logger;
        private readonly IMemoryCache _memoryCache;
        private string _progressKey;
        private int[] errorReasonCodes = new int[10] { 9, 19, 29, 39, 49, 59, 69, 79, 89, 99 };

        #region Controller

        public CalculationController(ILogger<CalculationController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        #endregion

        #region API Endpoints

        [HttpPost]
        public Guid StartCalculation([FromForm]string firstName, [FromForm] string lastName)
        {
            Guid uniqueId = System.Guid.NewGuid();
            _progressKey = uniqueId.ToString();
            Task.Run(() => StartCalucation(firstName, lastName));
            return uniqueId;
        }

        [HttpGet]
        public JsonResult GetStatus(string id)
        {

            _progressKey = id;
            StatusObject result = this.Progress;

            if(result.StatusCode == Controllers.StatusCode.Completed.ToString() || 
                result.StatusCode == Controllers.StatusCode.Failed.ToString())
                InvalidateCache();

            return new JsonResult(result);
        }

        #endregion

        #region Properties

        private StatusObject Progress
        {
            get
            {
                StatusObject progress;
                if (!_memoryCache.TryGetValue(_progressKey, out progress))
                {
                    var cacheExpiryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromSeconds(20)
                    };
                    _memoryCache.Set(_progressKey, new StatusObject { Percentage = 0, StatusCode = Controllers.StatusCode.Invalid.ToString(), Result = null }, cacheExpiryOptions);
                }
                return (StatusObject)_memoryCache.Get(_progressKey);
            }
            set
            {
                _memoryCache.Set(_progressKey, value);
            }
        }

        #endregion

        #region Methods

        private void StartCalucation(string firstName, string lastName)
        {
            Progress = new StatusObject { Percentage = 0, StatusCode = Controllers.StatusCode.Started.ToString(), Result = null };

            Random random = new Random();
            int interval = random.Next(220, 600);
            int errorNumber = random.Next(1, 100);

            IList<string> items = new List<string>();

            for (int i = 1; i <= 100; i++)
            {

                Thread.Sleep(interval);
                Progress = new StatusObject { Percentage = ((i + 9) / 10) * 10, StatusCode = i < 100 ? Controllers.StatusCode.Running.ToString() : Controllers.StatusCode.Completed.ToString(), Result = i < 100 ? null : items };


                if (errorReasonCodes.Contains(errorNumber))
                {
                    Progress = new StatusObject { Percentage = 33, StatusCode = Controllers.StatusCode.Failed.ToString(), Result = null };
                    break;
                }


                if (i % 3 == 0 && i % 5 == 0)
                {
                    items.Add(string.Format("{0} {1}", firstName, lastName));
                    continue;
                }

                if (i % 3 == 0)
                {
                    items.Add(firstName);
                    continue;
                }

                if (i % 5 == 0)
                {
                    items.Add(lastName);
                    continue;
                }
                items.Add(i.ToString());
            }
        }

        private void InvalidateCache()
        {
            _memoryCache.Remove(_progressKey);
        }

        #endregion  
    }

    #region STATUS 

    public class StatusObject
    {
        public int Percentage { get; set; }
        public string StatusCode { get; set; }
        public IList<string> Result { get; set; }
    }

    public enum StatusCode
    {
        NewRequest = 0,
        Started = 1,
        Running = 2,
        Completed = 3,
        Invalid = 99,
        Failed = 100
    }

    #endregion 
}
