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
            if (error) throw error;
            return containers ? containers.length > 0 : false;
        });
    };
    
    self.putBlob = function(blobName, text) {
        self.blobService.createBlockBlobFromText(self.containerName, blobName, text, function(error) {
            if (error) throw error;
        });
    };

    self.deleteBlob = function(blobName) {
        self.blobService.deleteBlob(self.containerName, blobName);
    };

    if (!self.doesContainerExist(self.containerName))
        throw new Error('The container ' + self.containerName + ' does not exist.');
}