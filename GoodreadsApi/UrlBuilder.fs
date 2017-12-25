module UrlBuilder

open OAuth

let requestTokenUrl = "https://www.goodreads.com/oauth/request_token"

let accessTokenUrl = "https://www.goodreads.com/oauth/access_token"

let authorizeUrl = sprintf "https://www.goodreads.com/oauth/authorize?oauth_token=%s&oauth_callback=%s"

let userUrl = "https://www.goodreads.com/api/auth_user"

let reviewsOnPageUrl accessData userId shelf sort perPage pageNumber = 
    sprintf "https://www.goodreads.com/review/list/%i.xml?key=%s&v=2&shelf=%s&sort=%s&per_page=%i&page=%i" userId accessData.ClientKey 
        shelf sort perPage pageNumber

let bookDetailUrl accessData bookId = sprintf "https://www.goodreads.com/book/show/%i.xml?key=%s" bookId accessData.ClientKey 
