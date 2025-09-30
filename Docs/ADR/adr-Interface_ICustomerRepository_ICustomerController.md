## Status: Accettato

# Decisione sulla necessità di aggiungere ICustomerRepository e ICustomerController

## Context and Problem Statement

Nel contesto della gestione dati del cliente, si è dibattuto se aggiungere o meno ICustomerRepository e ICustomerController, favorendo sicuramente estendibilità e testabilità, ma rischiando di infrangere le linee guida di Clean Architecutre e DDD

## Considered Options

* Mantenere le interfacce ICustomerRepository e ICustomerController
* Eliminatre le interfacce ICustomerRepository e ICustomerController

## Decision Outcome

Abbiamo mantenuto le interfacce, reputantandole in linea con le due guide architetturali sopra citate

### Consequences

Il sistema è più solido, estendibile e testabile senza violare Clean Architecture e DDD