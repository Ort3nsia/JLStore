## Status: Accettato

# Decisione sulla necessità di mantenere l'entità Customer

## Context and Problem Statement

Nel contesto della gestione dei dati del cliente e delle operazioni di validazione, ci siamo trovati a decidere se mantenere l'entità Customer nel nostro modello di dominio, poiché inizialmente sembrava superflua dato che molte delle sue funzionalità erano percepite come semplici operazioni sui dati. Tuttavia, considerando la logica di validazione implementata al suo interno, e l'importanza di avere un'entità ben definita nel contesto del Domain-Driven Design (DDD), abbiamo deciso di mantenere l'entità Customer.

## Considered Options

* Mantenere l'entità Customer, continuando ad utilizzare la sua logica di validazione e gestione dei dati.
* Eliminare l'entità Customer e trattare i dati come strutture semplici senza logica di validazione interna.

## Decision Outcome

Abbiamo deciso di mantenere l'entità Customer, poiché la sua presenza è giustificata dalla logica di validazione che è parte integrante del nostro dominio.

### Consequences

Mantenere l'entità Customer consente di centralizzare la logica di validazione e gestione dei dati al suo interno. Questo approccio permette anche di garantire che la logica di business sia sempre valida e coerente, evitando la duplicazione della logica di validazione in più parti del sistema. Tuttavia, la scelta di mantenere l'entità aumenta la complessità rispetto a trattare i dati come strutture semplici, ma offre il vantaggio di avere un modello di dominio robusto e facilmente estendibile. Se in futuro dovessero essere aggiunti comportamenti complessi legati al cliente, l'entità Customer risulta essere il punto centrale per queste modifiche, consentendo un'evoluzione del sistema in maniera ordinata e scalabile.