## Status: Rifiutato

# Decisione sull'uso delle interfacce per l'entità Customer

## Context and Problem Statement

Nel contesto della entità architetturali si è dibattuto se aggiungere o meno l'interfaccia ICustomer, così da permettere una maggiore estendibilità in caso di aggiunta di nuovi tipi di clienti. L'altra opzione sarebbe stata di non aggiungere l'interfaccia e preferire l'ereditarietà in quel caso.

## Considered Options

* Aggiungere l'interfaccia ICustomer
* Non aggiungere l'interfaccia ICustomer

## Decision Outcome

Si è scelto di non aggiungere l'interfaccia, così da non fare overengenieering, prediligendo dunque l'erediterierà come soluzione in caso di clienti futuri diversi. 

### Consequences

Il sistema è rimasto stabile ed in linea con le guide fornite da Clean Architecture e DDD