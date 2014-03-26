var azure = require('azure');

function BlobWriter(storageAccount, storageAccessKey, containerName) {
    var self = this;
    self.containerName = containerName;
    self.blobService = azure.createBlobService(storageAccount, storageAccessKey);

    self.doesContainerExist = function(containerName) {
        var options = {
            prefix: containerName,
            maxResults: 1
        };

        self.blobService.listContainers(options, function(error, containers) {
            return containers ? containers.length > 0 : false;
        });
    };
    
    self.putBlob = function(blobName, contentStream) {
        self.blobService.putBlockBlobFromFile(self.containerName, blobName, contentStream, function(error) {
            if (error)
                throw new Error(error);
        });
    };

    self.deleteBlob = function(blobName) {
        self.blobService.deleteBlob(self.containerName, blobName);
    };

    if (!self.doesContainerExist(self.containerName))
        throw new Error('The container ' + self.containerName + ' does not exist.');
}