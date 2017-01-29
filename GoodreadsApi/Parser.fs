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
    
let parseRatingDistribution (distributionValue:string) =
    let parsePair (x:string) =
        let parts = x.Split([|':'|])
        (parts.[0], int parts.[1])
    distributionValue.Split([|'|'|]) |> Seq.map parsePair |> dict

let parseWork workElement=
    {
        Id = elementValue workElement "id" |> int
        BooksCount = elementValue workElement "books_count" |> int
        BestBookId = elementValue workElement "best_book_id" |> int
        ReviewsCount = elementValue workElement "reviews_count" |> int
        RatingSum = elementValue workElement "ratings_sum" |> int
        RatingsCount = elementValue workElement "ratings_count" |> int
        TextReviewsCount = elementValue workElement "id" |> int
        OriginalPublicationYear = elementValue workElement "original_publication_year" |> parseOptionInt
        OriginalPublicationMonth = elementValue workElement "original_publication_month" |> parseOptionInt
        OriginalPublicationDay  = elementValue workElement "original_publication_day" |> parseOptionInt
        OriginalTitle = elementValue workElement "original_title"
        OriginalLanguageId= elementValue workElement "original_language_id" |> parseOptionInt
        MediaType = elementValue workElement "media_type"
        RatingDistribution  =elementValue workElement "rating_dist" |> parseRatingDistribution
        DescriptionUserId = elementValue workElement "desc_user_id" |> int
    }

let parseSeriesWork seriesWorkElement=
    {
        Id = elementValue seriesWorkElement "id" |> int
        UserPosition = elementValue seriesWorkElement "user_position" |> int
        Series = 
            let seriesElement = element seriesWorkElement "series" 
            {
                Id = elementValue seriesElement "id" |> int
                Title = elementValue seriesElement "title" 
                Description = elementValue seriesElement "description" 
                Note = elementValue seriesElement "note" 
                SeriesWorksCount = elementValue seriesElement "series_works_count" |> int
                PrimaryWorkCount = elementValue seriesElement "primary_work_count" |> int
                Numbered = elementValue seriesElement "numbered" |> parseBool
            }
    }

let parseSimilarBook bookElement=
    let parseAuthor e = (int (elementValue e "id"),  elementValue e "name")

    { SimilarBook.Id = elementValue bookElement "id" |> int
      Title = elementValue bookElement "title"
      Authors =  
        let authorsElement = element bookElement "authors"
        elements authorsElement "author" |> Seq.map parseAuthor |> Seq.toArray
    }

let parseBookDetail bookDetailResponse =
    let document = parseDocument bookDetailResponse
    let bookElement = element document "book"
    { 
        Id = elementValue bookElement "id" |> int
        Title = elementValue bookElement "title"
        Isbn = elementValue bookElement "isbn"
        Isbn13 = elementValue bookElement "isbn13"
        Asin = elementValue bookElement "asin"
        KindleAsin= elementValue bookElement "kindle_asin"
        MarketplaceId =  elementValue bookElement "marketplace_id"
        CountryCode = elementValue bookElement "country_code"
        ImageUrl = elementValue bookElement "image_url"
        SmallImageUrl = elementValue bookElement "small_image_url"
        PublicationYear = elementValue bookElement "publication_year" |> parseOptionInt
        PublicationMonth = elementValue bookElement "publication_month" |> parseOptionInt
        PublicationDay = elementValue bookElement "publication_day" |> parseOptionInt
        Publisher = elementValue bookElement "publisher"
        LaguageCode = elementValue bookElement "language_code"
        IsEbook= elementValue bookElement "is_ebook" |> parseBool
        Description = elementValue bookElement "description"
        Work = element bookElement "work" |>  parseWork
        AverageRating = elementValue bookElement "average_rating" |> float
        NumPages = elementValue bookElement "num_pages" |> parseOptionInt
        Format =  elementValue bookElement "format"
        EditionInformation=  elementValue bookElement "edition_information"
        RatingsCount = elementValue bookElement "ratings_count" |> int
        TextReviewsCount= elementValue bookElement "text_reviews_count" |> int
        Url = elementValue bookElement "url"
        Link = elementValue bookElement "link"
        Authors = 
            let authorsElement = element bookElement "authors"
            elements authorsElement "author" |> Seq.map parseAuthor |> Seq.toArray
        PopularShelves = 
            let shelvesElement = element bookElement "popular_shelves"
            elements shelvesElement "shelf" |> Seq.map (fun e -> (attributeValue e "name",  int (attributeValue e "count"))) |> Seq.toArray
        SeriesWorks =  
            let seriesElement = element bookElement "series_works"
            elements seriesElement "series_work" |> Seq.map parseSeriesWork |> Seq.toArray
        SimilarBooks =  
            let similarBooksElement = element bookElement "similar_books"
            elements similarBooksElement "book" |> Seq.map parseSimilarBook |> Seq.toArray
    }
