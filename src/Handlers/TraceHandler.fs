module Ruto.Handlers.TraceHandler

open Suave.Http
open Suave.Successful
open Confluent.Kafka

module RawProducer = Ruto.Messaging.Producers.RawProducer
module Logger = Ruto.Utils.Logger

let trace =
  fun (ctx: HttpContext) ->
    async {
      let reqData = ctx.request.rawForm |> UTF8.toString
      let! res = RawProducer.Deliver reqData

      let statusStr =
        (match res.Status with
         | PersistenceStatus.NotPersisted -> "Not Persisted"
         | PersistenceStatus.Persisted -> "Persisted"
         | PersistenceStatus.PossiblyPersisted -> "Possibly Persisted"
         | _ -> "Unknown")

      Logger.Debug (sprintf "Kafka delivery persistance status: %s" statusStr)
      return! OK "OK" ctx
    }
