## Status: Rifiutato da GIORGIO MASCI il miglior cliente del mondo

# Decisione sulla separazione delle operazioni CRUD in quattro servizi separati

## Context and Problem Statement

Nel contesto della gestione dei servizi per le operazioni CRUD, ci siamo trovati a decidere se mantenere un singolo servizio che gestisse tutte le operazioni o se separare ogni operazione in quattro servizi distinti (CreateService, ReadService, UpdateService, DeleteService), rispettando il principio di Single Responsibility Principle (SRP). Dopo una valutazione approfondita, abbiamo deciso di dividere le operazioni in quattro servizi separati per migliorare la chiarezza e la manutenibilità del codice.

## Considered Options

* Mantenere un unico servizio che gestisse tutte le operazioni CRUD.
* Dividere le operazioni CRUD in quattro servizi separati (CreateService, ReadService, UpdateService, DeleteService), seguendo il principio SRP.

## Decision Outcome

Abbiamo deciso di mantenere le operazioni CRUD in un'unico servizio dato che attualmente non fa altro che girare le richieste alla repository, senza applicare logica di business.

### Consequences

Mantenendo tutto in una singola classe, in futuro sara' possibile implementare le regole di business.