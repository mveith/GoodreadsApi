module XmlUtils

open System.Xml.Linq

let xn = XName.Get

let parseDocument xml = 
    let document = XDocument.Parse xml
    document.Root
    
let element (container : XContainer) xname =
    container.Element (xn xname)
    
let elementValue (container : XContainer) xname =
    let element = container.Element (xn xname)
    element.Value

let attribute (element : XElement) xname =
    element.Attribute (xn xname)
    
let attributeValue (element : XElement) xname =
    let element = attribute element xname
    element.Value
    