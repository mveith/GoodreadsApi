module Model

open System

type User = 
    { Id : int
      Link : string
      Name : string }

type Author=
    { Id : int
      Name : string
      Role : string
      ImageUrl : string
      SmallImageUrl : string
      Link : string
      AverageRating : decimal
      RatingsCount : int
      TextReviewsCount : int }

type Book=
    { Id : int
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
      ReviewShelfId : int option
      Sortable : bool }

type Review =
    { Id : int
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
      Owned : int }

type Reviews =
    { Start : int
      End : int
      Total : int
      Reviews : Review[] }