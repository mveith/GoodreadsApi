module GoodreadsApi

open OAuth
open Parser
open UrlBuilder

let getAuthorizationData clientKey clientSecret callbackUrl =
    let (token, tokenSecret) = getAuthorizatonToken clientKey clientSecret requestTokenUrl
    (authorizeUrl token callbackUrl, token, tokenSecret) 

let getAccessToken clientKey clientSecret token tokenSecret =
    getAccessToken clientKey clientSecret accessTokenUrl token tokenSecret

let getAccessData = getAccessData
        
let getUrlContentWithAccessData url accessData =  getUrlContentWithAccessData accessData url

let getUser accessData = userUrl |> getUrlContentWithAccessData accessData |> parseUser

let getReviewsOnPage accessData userId shelf sort perPage pageNumber = 
    reviewsOnPageUrl accessData userId shelf sort perPage pageNumber |> getUrlContentWithAccessData accessData |> parseReviews

let getAllReviews accessData userId shelf sort = 
    let perPage = 200
    let getReviewsOnPage = getReviewsOnPage accessData userId shelf sort perPage
    let firstPageReviews = getReviewsOnPage 1
    
    let pagesCount = 
        float firstPageReviews.Total / float perPage
        |> ceil
        |> int
    
    let first = seq firstPageReviews.Reviews
    
    let others = 
        [ 2..pagesCount ]
        |> Seq.map getReviewsOnPage
        |> Seq.collect (fun pageReviews -> pageReviews.Reviews)
    Seq.concat [| first; others |]

let getBookDetail accessData bookId=
    bookDetailUrl accessData bookId |> getUrlContentWithAccessData accessData |> parseBookDetail