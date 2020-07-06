using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PhotoBank.DataAccess;
using PhotoBank.Photo.Contracts;
using PhotoBank.Photo.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.QueueLogic.Manager;

namespace PhotoBank.Photo.Service
{
    public class PhotoWorker : BackgroundService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IQueueManager _queueManager;
        private readonly ILogger<PhotoWorker> _logger;

        public PhotoWorker(IRepositoryFactory repositoryFactory, IQueueManager queueManager, ILogger<PhotoWorker> logger)
        {
            _repositoryFactory = repositoryFactory;
            _queueManager = queueManager;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = _queueManager.Wait(PhotoSettings.PhotoInputQueue);
                if (message is GetPhotosInputMessage)
                {
                    await Task.Factory.StartNew(() => ProcessGetPhotosInputMessage((GetPhotosInputMessage)message));
                }
            }
        }

        private void ProcessGetPhotosInputMessage(GetPhotosInputMessage inputMessage)
        {
            var photos = _repositoryFactory.Get<IPhotoRepository>().GetUserPhotos(inputMessage.UserId);
            var outputMessage = new GetPhotosOutputMessage(inputMessage.Guid, OutputMessageResult.Success)
            {
                PhotoIds = photos.Select(x => x.Id).ToList()
            };
            _queueManager.Send(PhotoSettings.PhotoOutputQueue, outputMessage);
        }
    }
}
