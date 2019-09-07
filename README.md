
# ProblemNet
ProblemNET is a middleware library that implements rfc7807. The middleware catches all exceptions and handles according to exception type.    
For more information: https://tools.ietf.org/html/rfc7807
## Installation
>  Install-Package ProblemNet  
## Configuration
```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddProblemDetails(cfg => { cfg.DefaultTypeBaseUri = "https://sample.com/problemdetails/"; });

            services.AddMvc().
                     UseProblemDetailsInvalidModelStateResponseFactory();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseProblemDetailsExceptionHandler();
        ..
        ..
        ...
    }
}
```

## Usage
### Creating Problems
There are different ways to create problems.  

 - **Simple**

    ```csharp
     throw new ProblemDetailsException()
                      {
                              Type = "user-not-exists",
                              Title = "User Not Exists",
                              Status = (int)HttpStatusCode.NotFound,
                              Detail = "Users cannot be found"
                      };
    ```
    Will produce this response
    ```json
      "type": "https://userservice.com/problemdetails/user-not-exists",
      "title": "User Not Exists",
      "status": 404,
      "detail": "Users cannot be found",
      "instance": "/api/Samples/problem-details-simple"
    ```
 - **Additional Members**
    ```csharp
     throw new ProblemDetailsException
                      {
                              Status = (int)HttpStatusCode.NotFound,
                              Title = $"Users cannot be found",
                              Extensions =
                              {
                                      new KeyValuePair<string, object>("Order", new { OrderId = 1, OrderDate = DateTime.UtcNow, Status = "Approved" }),
                                      new KeyValuePair<string, object>("UserId", 123)
                              }
                      };
    ```
	Will produce this
	 ```json
      "type": "https://userservice.com/problemdetails/",
      "title": "Users cannot be found",
      "status": 404,
      "instance": "/api/Samples/problem-details-additional-members",
      "order": {
        "orderId": 1,
        "orderDate": "2019-09-07T14:24:45.1783264Z",
        "status": "Approved"
      },
      "userId": 123
	  ```
 - **Custom Validation Exception**

    ```csharp
    var validationProblems = new List<ValidationProblem>
                                         {
                                                 new ValidationProblem("FirstName", "FirstName length must be less than 50 characters", "FirstName should only contain letters"),
                                                 new ValidationProblem("Email", "Email address length must be greater than 5 characters", "Email address must be email format")
                                         };
    throw new ValidationProblemDetailsException(validationProblems.ToArray())
                      {
                              Detail = "Error while creating account. Please see errors for detail.",
                              Status = (int)HttpStatusCode.BadRequest,
                              Title = "Register Error"
                      };
                
    ```
	Will produce this
	 ```json
	   ```
 - **Exceptions Except Problem Details**

      ```csharp
    throw new NullReferenceException();
      ```
	Will produce the following output. If the environment is **development**, the content of the **exception property** will not be empty.
	 ```json
      "exception": {  },
      "type": "https://httpstatuses.com/500",
      "title": "Internal Server Error",
      "status": 500,
      "detail": "An error occurred while processing your request",
      "instance": "/api/Samples/unhandled-exception"
	   ```
### Response Headers
The same http response headers will be produced regardless of exception type on the lines below. Middleware also ensures that problem responses never cached.
 ```json

    cache-control: no-cache, no-store, must-revalidate 
    content-type: application/problem+json; charset=utf-8 
    date: Sat, 07 Sep 2019 14:03:55 GMT 
    expires: 0 
    pragma: no-cache 
    server: Kestrel 
    transfer-encoding: chunked 

  ```