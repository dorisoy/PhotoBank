﻿using System;
using System.IO;
using PhotoBank.Photo.Contracts;
using PhotoBank.Photo.Service.Data;
using PhotoBank.QueueLogic.Contracts;
using PhotoBank.Service.Common.MessageProcessors;

namespace PhotoBank.Photo.Service.MessageProcessors
{
    [MessageProcessor(MessageType = typeof(GetPhotoInputMessage))]
    public class GetPhotoInputMessageProcessor : MessageProcessor
    {
        public override void Execute()
        {
            var inputMessage = GetMessageAs<GetPhotoInputMessage>();
            GetPhotoOutputMessage outputMessage;
            var photo = _context.RepositoryFactory.Get<IPhotoRepository>().GetPhoto(inputMessage.PhotoId);
            var fullPhotoPath = photo != null ? Path.Combine(PhotoSettings.RootPhotoPath, photo.Path) : "";
            if (photo != null && File.Exists(fullPhotoPath))
            {
                var fileContent = File.ReadAllBytes(fullPhotoPath);
                var fileBase64Content = Convert.ToBase64String(fileContent);
                outputMessage = new GetPhotoOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Success)
                {
                    PhotoId = photo.Id,
                    FileBase64Content = fileBase64Content,
                    CreateDate = photo.CreateDate
                };
            }
            else
            {
                outputMessage = new GetPhotoOutputMessage(inputMessage.ClientId, inputMessage.ChainId, OutputMessageResult.Error);
            }
            _context.QueueManager.SendMessage(PhotoSettings.PhotoOutputQueue, outputMessage);
        }
    }
}
