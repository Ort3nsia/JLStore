## Status: Accettato

# Decisione sull'uso delle interfacce per i servizi

## Context and Problem Statement

Nel contesto della gestione dei servizi per le operazioni CRUD e la mappatura tramite Automapper, ci siamo trovati a decidere se aggiungere o meno le interfacce per i servizi (casi d'uso). Dopo una valutazione, abbiamo ritenuto che l'introduzione delle interfacce non fosse necessaria, considerando che al momento le operazioni si limitano a semplici CRUD e mappature, senza la necessità di complessità aggiuntiva o di evoluzione del sistema che giustifichi l'astrazione.

## Considered Options

* Aggiungere una o più interfacce per i servizi (casi d'uso).
* Mantenere l'attuale progettazione senza interfacce.

## Decision Outcome

La scelta e' quella di implementare una singola interfaccia al momento, nonostante non vi siano regole di business. Questo viene fatto per consentire in futuro di estendere i vari servizi.

### Consequences

Aprendo i servizi alla estendibilita', si va incontro ad una complessita' di codice maggiore.