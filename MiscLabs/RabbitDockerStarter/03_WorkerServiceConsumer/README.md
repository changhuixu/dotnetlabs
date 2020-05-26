# RabbitMQ on Docker Starter: Using Worker Service as Consumer

```Docker
# Start both RabbitMQ and the EmailWorker in containers
docker-compose up --build

# Start RabbitMQ only, then run the EmailWorker locally
docker-compose run --service-ports rabbitmq

```

## Demo

Consuming RabbitMQ messages using an ASP.NET Core Worker Service application.

```bash
admin:admin123 (admin account)
ops0:ops0 (msg producer account)
ops1:ops1 (msg consumer account)
```

![rabbitmq starter](./rabbitmq-starter.gif)
