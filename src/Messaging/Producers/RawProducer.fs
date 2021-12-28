module Ruto.Messaging.Producers.RawProducer

open Confluent.Kafka
open FsKafka

module Config = Ruto.Utils.Config
module Logger = Ruto.Utils.Logger

let batching =
  Linger (System.TimeSpan.FromMilliseconds 10.)

let producerConfig () =
  Logger.Debug (
    sprintf "Raw Telemetry Consumer - Kafka clientId: %s; bootstrapServers: %s" Config.KafkaClientId Config.KafkaHost
  )

  KafkaProducerConfig.Create (Config.KafkaClientId, Config.KafkaHost, Acks.All, batching)

let producer =
  KafkaProducer.Create (Logger.log, producerConfig (), "raw-telemetry")

let Deliver msg =
  let key = System.Guid.NewGuid().ToString ()
  producer.ProduceAsync (key, msg)
