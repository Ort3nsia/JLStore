## Status: Rifiutato

# Decisione sull'uso delle interfacce per i servizi

## Context and Problem Statement

Nel contesto della gestione dei servizi per le operazioni CRUD e la mappatura tramite Automapper, ci siamo trovati a decidere se aggiungere o meno le interfacce per i servizi (casi d'uso). Dopo una valutazione, abbiamo ritenuto che l'introduzione delle interfacce non fosse necessaria, considerando che al momento le operazioni si limitano a semplici CRUD e mappature, senza la necessità di complessità aggiuntiva o di evoluzione del sistema che giustifichi l'astrazione.

## Considered Options

* Aggiungere una o più interfacce per i servizi (casi d'uso).
* Mantenere l'attuale progettazione senza interfacce.

## Decision Outcome

La scelta di mantenere la struttura attuale senza interfacce è giustificata dalla semplicità del sistema e dalla mancanza di necessità immediata di flessibilità o complessità aggiuntiva.

### Consequences

Tuttavia, questa decisione potrebbe rendere più difficile l'estensione del sistema in futuro, e ridurre la testabilità a lungo termine. Se il sistema dovesse evolvere verso operazioni più complesse o un'architettura più scalabile, si dovrebbero rivalutare le scelte architetturali, inclusa l'introduzione di interfacce.