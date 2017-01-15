module Parser

open XmlUtils
open Model

let parseUser userResponse=
    let document = parseDocument userResponse
    let userElement = element document "user"
    let id = attributeValue userElement "id" |> int
    let name = elementValue userElement "name"
    let link = elementValue userElement "link"

    { Id = id; Name = name; Link = link}
