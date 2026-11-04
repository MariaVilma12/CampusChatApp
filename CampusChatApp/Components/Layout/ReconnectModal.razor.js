// Set up event handlers
const reconnectModalElement = document.getElementById("components-reconnect-modal");
const retryButton = document.getElementById("components-reconnect-button");
const resumeButton = document.getElementById("components-resume-button");
const reconnectModal = reconnectModalElement instanceof HTMLDialogElement ? reconnectModalElement : null;

if (reconnectModal) {
    reconnectModal.addEventListener("components-reconnect-state-changed", handleReconnectStateChanged);
}

if (retryButton) {
    retryButton.addEventListener("click", retry);
}

if (resumeButton) {
    resumeButton.addEventListener("click", resume);
}

function handleReconnectStateChanged(event) {
    const state = event?.detail?.state;
    if (!reconnectModal || !state) {
        return;
    }

    if (state === "show") {
        reconnectModal.showModal();
    } else if (state === "hide") {
        reconnectModal.close();
    } else if (state === "failed") {
        document.addEventListener("visibilitychange", retryWhenDocumentBecomesVisible);
    } else if (state === "rejected") {
        location.reload();
    }
}

async function retry() {
    document.removeEventListener("visibilitychange", retryWhenDocumentBecomesVisible);

    try {
        const blazor = globalThis.Blazor;
        if (!blazor) {
            location.reload();
            return;
        }

        // Reconnect will asynchronously return:
        // - true to mean success
        // - false to mean we reached the server, but it rejected the connection (e.g., unknown circuit ID)
        // - exception to mean we didn't reach the server (this can be sync or async)
        const successful = await blazor.reconnect();
        if (!successful) {
            // We have been able to reach the server, but the circuit is no longer available.
            // We'll reload the page so the user can continue using the app as quickly as possible.
            const resumeSuccessful = await blazor.resumeCircuit();
            if (!resumeSuccessful) {
                location.reload();
            } else if (reconnectModal) {
                reconnectModal.close();
            }
        }
    } catch {
        // We got an exception, server is currently unavailable
        document.addEventListener("visibilitychange", retryWhenDocumentBecomesVisible);
    }
}

async function resume() {
    try {
        const blazor = globalThis.Blazor;
        if (!blazor) {
            location.reload();
            return;
        }

        const successful = await blazor.resumeCircuit();
        if (!successful) {
            location.reload();
        }
    } catch {
        if (reconnectModal) {
            reconnectModal.classList.replace("components-reconnect-paused", "components-reconnect-resume-failed");
        }
    }
}

async function retryWhenDocumentBecomesVisible() {
    if (document.visibilityState === "visible") {
        await retry();
    }
}
