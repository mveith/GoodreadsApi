# GoodreadsApi
[Goodreads API](https://www.goodreads.com/api) wrapper written in F#.

## Sample of Use: Authorize
```fsharp
let (authorizationUrl, token, tokenSecret) = getAuthorizationData clientKey clientSecret clientSideUrl
```

## Sample of Use: Access Token (AccessData)
```fsharp
let (token, tokenSecret) = getAccessToken clientKey clientSecret authorizationToken authorizationTokenSecret
let accessData = getAccessData clientKey clientSecret token tokenSecret
```

## Sample of Use: Get User (ID, Name)
```fsharp
let user = getUser accessData
```

## Sample of Use: Get Reviews Count (Shelve = read)
```fsharp
let user = getUser accessData
let reviews = getReviewsCount accessData user.Id "read"
```

## Sample of Use: Get Reviews Page (Shelve = read, Sort = date read, Reviews per page = 10, Page number  = 13)
```fsharp
let user = getUser accessData
let reviews = getReviewsOnPage accessData user.Id "read" "date_read" 10 13
```

## Sample of Use: Get Reviews (Shelve = read, Sort = date read)
```fsharp
let user = getUser accessData
let reviews = getAllReviews accessData user.Id "read" "date_read"
```

## Sample of Use: Get Book detail (Book ID = 53732 (Frank Herbert: Dune))
```fsharp
let bookDetail = getBookDetail accessData 53732
```
