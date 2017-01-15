module Parser

open XmlUtils
open Model
open System.Xml.Linq

let parseUser userResponse=
    let document = parseDocument userResponse
    let userElement = element document "user"
    let id = attributeValue userElement "id" |> int
    let name = elementValue userElement "name"
    let link = elementValue userElement "link"
    { User.Id = id; Name = name; Link = link}
    
let goodreadsDataFormat = "ddd MMM dd HH:mm:ss zzz yyyy"

let parseDate s = 
    System.DateTime.ParseExact(s, goodreadsDataFormat, System.Globalization.CultureInfo.InvariantCulture)
    
let parseOptionDate (s:string) = 
    match s with
    | "" -> None
    | value -> Some (parseDate value)
    
let parseOptionInt (s:string) =
    match s with
    | "" -> None
    | value -> Some (int value)

let parseOptionIntFromOption (s:string option) =
    match s with
    | Some "" -> None
    | Some value -> Some (int value)
    | None -> None

let parseBool (s:string)=
    let (isBool, boolValue) = System.Boolean.TryParse s
    if isBool then boolValue else false
    
let parseBoolFromOption (s:string option)=
    match s with
    | Some s ->
        let (isBool, boolValue) = System.Boolean.TryParse s
        if isBool then boolValue else false
    | None -> false

let parseAuthor (authorElement:XElement)=
    { Id = elementValue authorElement "id" |> int
      Name = elementValue authorElement "name"
      Role = elementValue authorElement "role"
      ImageUrl = elementValue authorElement "image_url"
      SmallImageUrl = elementValue authorElement "small_image_url"
      Link = elementValue authorElement "link"
      AverageRating = elementValue authorElement "average_rating" |> decimal
      RatingsCount = elementValue authorElement "ratings_count" |> int
      TextReviewsCount = elementValue authorElement "text_reviews_count" |> int}

let parseBook (bookElement:XElement)=    
    { Id = elementValue bookElement "id" |> int
      Isbn = elementValue bookElement "isbn"
      Isbn13 = elementValue bookElement "isbn13"
      TextReviewsCount = elementValue bookElement "text_reviews_count" |> int
      Title = elementValue bookElement "title"
      ImageUrl = elementValue bookElement "image_url"
      SmallImageUrl = elementValue bookElement "small_image_url"
      LargeImageUrl = elementValue bookElement "large_image_url"
      Link = elementValue bookElement "link"
      NumPages = elementValue bookElement "num_pages" |> parseOptionInt
      Format = elementValue bookElement "format"
      EditionInformation = elementValue bookElement "edition_information"
      Publisher = elementValue bookElement "publisher"
      PublicationDay = elementValue bookElement "publication_day" |> parseOptionInt
      PublicationMonth = elementValue bookElement "publication_month" |> parseOptionInt
      PublicationYear = elementValue bookElement "publication_year" |> parseOptionInt
      AverageRating = elementValue bookElement "average_rating" |> decimal
      RatingsCount = elementValue bookElement "ratings_count" |> int
      Description = elementValue bookElement "description"
      Authors =  
        let authorsElement = element bookElement "authors"
        elements authorsElement "author" |> Seq.map parseAuthor |> Seq.toArray
      Published = elementValue bookElement "published" |> parseOptionInt }

let parseShelf (shelfElement:XElement) =
    { Name = attributeValue shelfElement "name"
      Exclusive =  attributeValue shelfElement "exclusive" |> parseBool
      ReviewShelfId = attributeOptionValue shelfElement "review_shelf_id" |> parseOptionIntFromOption
      Sortable = attributeOptionValue shelfElement "sortable" |> parseBoolFromOption }

let parseReview (reviewElement:XElement)=
    { Id = elementValue reviewElement "id" |> int
      Rating = elementValue reviewElement "rating" |> int
      Votes = elementValue reviewElement "votes" |> int
      SpoilerFlag = elementValue reviewElement "spoiler_flag" |> parseBool
      SpoilersState = elementValue reviewElement "spoilers_state"
      RecommendedFor = elementValue reviewElement "recommended_for"
      RecommendedBy = elementValue reviewElement "recommended_by"
      StartedAt = elementValue reviewElement "started_at" |> parseOptionDate
      ReadAt = elementValue reviewElement "read_at" |> parseOptionDate
      DateAdded = elementValue reviewElement "date_added" |>  parseDate
      DateUpdated = elementValue reviewElement "date_updated" |>  parseDate
      ReadCount = elementValue reviewElement "read_count" |> parseOptionInt
      Body = elementValue reviewElement "body"
      CommentsCount = elementValue reviewElement "comments_count" |> int
      Url = elementValue reviewElement "url"
      Link = elementValue reviewElement "link"
      Book = element reviewElement "book" |> parseBook
      Shelves = 
        let shelvesElement = element reviewElement "shelves" 
        elements shelvesElement "shelf" |> Seq.map parseShelf |> Seq.toArray
      Owned = elementValue reviewElement "owned" |> int }

let parseReviews reviewsResponse=
    let document = parseDocument reviewsResponse
    let reviewsElement = element document "reviews"
    let reviews = elements reviewsElement "review" |> Seq.map parseReview |> Seq.toArray
    let reviewsElementIntAttribute xname = attributeValue reviewsElement xname |> int
    { Start = reviewsElementIntAttribute "start";  End = reviewsElementIntAttribute "end"; Total =  reviewsElementIntAttribute "total"; Reviews = reviews}