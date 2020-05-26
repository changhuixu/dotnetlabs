# RabbitMQ on Docker Starter: Queue Properties

```Docker
docker-compose up


# interactively
docker run --rm -it --hostname my-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management

# output
# node           : rabbit@my-rabbit
# home dir       : /var/lib/rabbitmq
# config file(s) : /etc/rabbitmq/rabbitmq.conf

# detached
docker run --rm -d --hostname my-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management
```

## Demo

A basic pub/sub model with one message producer and two consumers.

```bash
admin:admin123 (admin account)
ops0:ops0 (msg producer account)
ops1:ops1 (msg consumer account)
```

![rabbitmq starter](./rabbitmq-starter.gif)

## Queue Properties

Queues have properties that define how they behave. There is a set of mandatory properties and a map of optional ones:

- Name
- Durable (the queue will survive a broker restart)
- Exclusive (used by only one connection and the queue will be deleted when that connection closes)
- Auto-delete (queue that has had at least one consumer is deleted when last consumer unsubscribes)
- Arguments (optional; used by plugins and broker-specific features such as message TTL, queue length limit, etc)

## Declaration and Property Equivalence

Before a queue can be used it has to be declared. Declaring a queue will cause it to be created if it does not already exist. The declaration will have no effect if the queue does already exist and its attributes are the same as those in the declaration. When the existing queue attributes are not the same as those in the declaration a channel-level exception with code 406 (`PRECONDITION_FAILED`) will be raised.

## Optional Arguments

Optional queue arguments, also known as "_x-arguments_" because of their field name in the `AMQP 0-9-1` protocol, is a map (dictionary) of arbitrary key/value pairs that can be provided by clients when a queue is declared.

The map is used by various features and plugins such as

- Queue type (e.g. quorum or classic)
- Message and queue TTL
- Queue length limit
- Classic mirrored queue settings
- Max number of priorities
- Consumer priorities
- and so on.

Most optional arguments can be dynamically changed after queue declaration but there are exceptions. For example, queue type (`x-queue-type`) and max number of queue priorities (`x-max-priority`) must be set at queue declaration time and cannot be changed after that.

Optional queue arguments can be set in a couple of ways:

- To groups of queues using policies (recommended)
- On a per-queue basis when a queue is declared by a client

The former option is more flexible, non-intrusive, does not require application modifications and redeployments. Therefore it is highly recommended for most users. Note that some optional arguments such as queue type or max number of priorities can only be provided by clients because they cannot be dynamically changed and must be known at declaration time.

The way optional arguments are provided by clients varies from client library to client library but is usually an argument next to the `durable`, `auto_delete` and other arguments of the function (method) that declares queues.

## Message Ordering

Queues in RabbitMQ are ordered collections of messages. Messages are enqueued and dequeued (consumed) in the FIFO manner, although priority queues, sharded queues and other features may affect this.

## Durability

Durable queues are persisted to disk and thus survive broker restarts. Queues that are not durable are called transient. Not all scenarios and use cases mandate queues to be durable.

Durability of a queue does not make messages that are routed to that queue durable. If broker is taken down and then brought back up, durable queue will be re-declared during broker startup, however, only persistent messages will be recovered.

## Purge all unacked messages

You have to make consumer `ack` them (or `nack`) and only after that they will be removed. Alternatively you can shutdown consumers and purge the queue completely.
