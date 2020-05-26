# RabbitMQ on Docker Starter

## Medium Articles

### [Get Started with RabbitMQ on Docker](https://codeburst.io/get-started-with-rabbitmq-on-docker-4428d7f6e46b)

In this article, we will talk about how to quickly spin up RabbitMQ instances on Docker. We will go through two ways to run a RabbitMQ Docker image in a container: (1) using the `docker run` command; (2) using Docker Compose. Getting familiar with these two approaches should greatly accelerate your learning on RabbitMQ.

This blog covers demos 01 and 02.

## [Demo 01](./01_Server-named_Queues)

A basic pub/sub model with one message producer and two consumers. Exchange type: `fanout`. This demo shows how to use a exclusive queue to produce and consume messages.

## [Demo 02](./02_QueueProperties)

A basic queue based on route key. Exchange type: `topic`. This demo shows how to use a passive queue to produce and consume messages, as well as how to pass some basic properties along with the messages, thus consumers can verify those properties.

Some user accounts are set up and a definition file can be easily loaded to the RabbitMQ service using a `docker-compose` command.

## [Demo 03](./03_WorkerServiceConsumer)

Consuming RabbitMQ messages using an ASP.NET Core Worker Service. This project implements async operations for the `Received` event.

This demo also includes an example `docker-compose.yml` for the worker service and rabbitmq services.

## [Demo 04](./04_MultiContainers)

This demo includes a Web API project as the message publisher, and a Worker Service project as the message consumer. The API project and Worker Service project are connected by a RabbitMQ service. The `docker-compose.yml` orchestra these services in containers.
