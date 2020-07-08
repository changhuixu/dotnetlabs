# HttpClient sending Multipart form-data

## [Medium Article: Upload/Download Files Using HttpClient in C#](https://codeburst.io/upload-download-files-using-httpclient-in-c-f29051dea40c)

This solution includes two projects, `ClientSideApp` and `ServerSideApp`.

- The `ServerSideApp` project contains an API controller `FilesController`, which includes two actions for (1) uploading a file with Multipart form-data, and (2) downloading a file. The `ServerSideApp` project also contains a web page for uploading a file.

- The `ClientSideApp` project is a Console application that uploads a `*.txt` file to the API endpoint in the `ServerSideApp`, then download the file from the `ServerSideApp`. The `ClientSideApp` project demonstrates an example of sending multipart form-data using HttpClient.

The following gif image shows the screen recording of the working demo.

![httpclinet-file-upload-download](./httpclient-file-upload-download.gif)
