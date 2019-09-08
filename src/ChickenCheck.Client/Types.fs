namespace ChickenCheck.Client
open ChickenCheck.Domain.Session
open ChickenCheck.Domain

[<AutoOpen>]
module DomainError =
    let getClientErrorMsg error =
        match error with
        | Validation _ -> "Ogiltigt värde"
        | Database _ | Duplicate | NotFound | ConfigMissing _ -> "Serverfel"
        | Authentication _ | Login _ -> "Autentiseringsfel"
    type DomainError with
        member this.ErrorMsg = getClientErrorMsg this

type StringInput<'a> =
    | Valid of 'a
    | Invalid of string
    | Empty

module StringInput = 
    let inline tryValid input =
        match input with
        | Valid a ->
            let value = (^a : (member Value : string) a)
            true, value
        | Invalid value -> 
            false, value
        | Empty -> 
            false, ""

    let inline isValid input = input |> tryValid |> fst

module OptionalStringInput =
    let inline tryValid input = 
        match input with
        | Valid a ->
            let value = (^a : (member Value : string) a)
            true, value
        | Invalid value -> 
            false, value
        | Empty -> 
            true, ""

    let inline isValid input = input |> tryValid |> fst

type NumberInput<'a> =
    | Valid of 'a
    | Invalid of string
    | Empty

module NumberInput = 
    let inline tryValid input =
        match input with
        | Valid (NaturalNum value) ->
            true, (value |> string)
        | Invalid value -> 
            false, value
        | Empty -> 
            false, ""

    let inline isValid input = input |> tryValid |> fst

type ApiCallStatus =
    | NotStarted
    | Running
    | Completed
    | Failed of string

type SigninModel =
    { Email : StringInput<Email>
      Password : StringInput<Password>
      LoginStatus : ApiCallStatus } with
    member __.IsValid =
        match __.Email, __.Password with
        | StringInput.Valid _, StringInput.Valid _ -> true
        | _ -> false
    member __.IsInvalid = __.IsValid |> not
    static member Init =
        { Email = StringInput.Empty
          Password = StringInput.Empty 
          LoginStatus = NotStarted }

type ChickensPageModel =
    { Chickens : Chicken list
      FetchStatus : ApiCallStatus } with
      static member Init =
        { Chickens = [] 
          FetchStatus = NotStarted }

module Pages =
    open Elmish.Navigation

    type Page =
        | SigninPage of SigninModel
        | ChickensPage of ChickensPageModel

    module SigninPage =
        let init = SigninModel.Init |> SigninPage

    module ChickensPage =
        let init = ChickensPageModel.Init |> ChickensPage


    let signinPageUrl = "#signin"
    let chickensPageUrl = "#"

    let private toHash page =
        match page with
        | SigninPage _ -> signinPageUrl
        | ChickensPage _ -> chickensPageUrl

    let href route =
        route
        |> toHash

    let modifyUrl route =
        route
        |> toHash
        |> Navigation.modifyUrl

    let newUrl route =
        route
        |> toHash
        |> Navigation.newUrl

open Pages
type Model =
    { Session: Session option
      CurrentPage: Page }
