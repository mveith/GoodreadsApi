module Model

open System
open System.Collections.Generic

type User = 
    { Id : int64
      Link : string
      Name : string }

type Author=
    { Id : int64
      Name : string
      Role : string
      ImageUrl : string
      SmallImageUrl : string
      Link : string
      AverageRating : decimal
      RatingsCount : int
      TextReviewsCount : int }

type Book=
    { Id : int64
      Isbn : string
      Isbn13 : string
      TextReviewsCount : int
      Title : string
      ImageUrl : string
      SmallImageUrl : string
      LargeImageUrl : string
      Link : string
      NumPages : int option
      Format : string
      EditionInformation : string
      Publisher : string
      PublicationDay : int option
      PublicationMonth : int option
      PublicationYear : int option
      AverageRating : decimal
      RatingsCount : int
      Description : string
      Authors: Author[]
      Published : int option }

type Shelf = 
    { Name : string
      Exclusive : bool
      ReviewShelfId : int64 option
      Sortable : bool }

type Review =
    { Id : int64
      Rating : int
      Votes : int
      SpoilerFlag : bool
      SpoilersState: string
      RecommendedFor : string
      RecommendedBy : string
      StartedAt : DateTime option
      ReadAt : DateTime option
      DateAdded : DateTime
      DateUpdated : DateTime
      ReadCount : int option
      Body : string
      CommentsCount : int
      Url : string
      Link : string
      Book : Book
      Shelves : Shelf[]
      Owned : int option }

type Reviews =
    { Start : int
      End : int
      Total : int
      Reviews : Review[] }

type Work =
    { Id : int64
      BooksCount : int
      BestBookId : int64
      ReviewsCount : int
      RatingSum : int
      RatingsCount : int
      TextReviewsCount : int
      OriginalPublicationYear : int option
      OriginalPublicationMonth : int option
      OriginalPublicationDay  : int option
      OriginalTitle : string
      OriginalLanguageId : int64 option
      MediaType : string
      RatingDistribution : IDictionary<string, int>
      DescriptionUserId : int64 option
    }

type Series =
    { Id :int64
      Title : string
      Description : string
      Note : string
      SeriesWorksCount : int
      PrimaryWorkCount : int
      Numbered : bool
    }

type SeriesWork =
    { Id :int64
      UserPosition : string
      Series : Series
    }

type SimilarBook =
    { Id : int64
      Title : string
      Authors: (int * string)[]
    }

type BookDetail =
    { Id : int64
      Title : string
      Isbn : string
      Isbn13 : string
      Asin :string
      KindleAsin :string
      MarketplaceId : string
      CountryCode : string
      ImageUrl : string
      SmallImageUrl : string
      PublicationYear : int option
      PublicationMonth : int option
      PublicationDay : int option
      Publisher : string
      LaguageCode : string
      IsEbook: bool
      Description : string
      Work : Work
      AverageRating : float
      NumPages : int option
      Format : string
      EditionInformation : string
      RatingsCount : int
      TextReviewsCount : int
      Url : string
      Link : string
      Authors : Author[]
      PopularShelves : (string * int)[]
      SeriesWorks : SeriesWork[]
      SimilarBooks : SimilarBook[] }