# RabbitMQ on Docker Starter

## [Demo 01](./01_Server-named_Queues)

A basic pub/sub model with one message producer and two consumers. Exchange type: `fanout`. This demo shows how to use a exclusive queue to produce and consume messages.

## [Demo 02](./02_QueueProperties)

A basic queue based on route key. Exchange type: `topic`. This demo shows how to use a passive queue to produce and consume messages, as well as how to pass some basic properties along with the messages, thus consumers can verify those properties.

Some user accounts are set up and a definition file can be easily loaded to the RabbitMQ service using a `docker-compose` command.
