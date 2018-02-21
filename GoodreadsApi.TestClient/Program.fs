open GoodreadsApi
open System
open System.Threading

let clientKey = "<CLIENT KEY>"
let clientSecret = "<CLIENT SECRET>"

[<EntryPoint>]
let main argv = 

    let (authorizationUrl, token, tokenSecret) = getAuthorizationData clientKey clientSecret "http://getpocket.com"
    System.Diagnostics.Process.Start(authorizationUrl)
    Thread.Sleep(TimeSpan.FromSeconds(30.0))
    let (token, tokenSecret) = getAccessToken clientKey clientSecret token tokenSecret
    let accessData = OAuth.getAccessData clientKey clientSecret token tokenSecret
    let user = getUser accessData
    
    let reviews = getReviewsOnPage accessData user.Id "read" "date_read" 1000 1
     
    printfn "%A" argv
    0 // return an integer exit code
