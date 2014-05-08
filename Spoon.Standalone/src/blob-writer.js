var azure = require('azure');

function BlobWriter(storageAccount, storageAccessKey, containerName) {
    var self = this;
    self.containerName = containerName;
    self.blobService = azure.createBlobService(storageAccount, storageAccessKey);

    self.clearContainer = function() {
        self.blobService.listBlobs(self.containerName, function(error, blobs) {
            if (error) throw error;
            for (var index in blobs) {
                self.deleteBlob(blobs[index].name);
            }
        });
    };

    self.putBlob = function(blobName, text) {
        self.blobService.createBlockBlobFromText(self.containerName, blobName, text, {contentType: 'text/html'}, function(error) { if (error) throw error; });
    };

    self.deleteBlob = function(blobName) {
        self.blobService.deleteBlob(self.containerName, blobName, function(error) { if (error) throw error; });
    };
}

module.exports = BlobWriter;