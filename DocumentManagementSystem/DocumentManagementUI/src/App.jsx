/*//import { useState } from 'react'
import './App.css'

// install react router for navigation
// npm install react-router-dom

function App() {
    //    const [count, setCount] = useState(0)

    function Documents() {
        return (
            <>
                <p>
                    <button onClick={changeCurrentDoc}>This is a Document</button><br />
                    <button onClick={changeCurrentDoc}>Document A</button><br/>
                    <button onClick={changeCurrentDoc}>Document B</button><br/>
                </p>
            </>
        );
    }

    function CurrentDocument() {
        return (
            <>
                <h2> This is a document </h2>
                <p>
                    This is a generated summary of the document contents
                </p>
            </>
        );
    }

    function changeCurrentDoc() {
        // figure out how to pass parameters and actually change displayed doc
        console.log("changing selected document");
    }
    function updateDoc() {
        // handle document update
        console.log("update current document");
    }
    function deleteDoc() {
        // handle document deletion
        console.log("delete current document");
    }
    function metaDoc() {
        // handle retrieval and display of document metadata
        console.log("display metadata");
    }

    function uploadDoc(e) {
        e.preventDefault();
        console.log("uploading document");
    }

  return (
    <>
          <h1>Document Management System</h1>

          <div>

              <div>
                  <div style={{ display: "inline-block", verticalAlign: "top", width: "10%", height: "60%" }} >
                      <Documents/>
                  </div>
                  <div style={{ display: "inline-block", verticalAlign: "top", width: "70%", height: "60%" }} >
                      <CurrentDocument/>
                  </div>
                  <div style={{ display: "inline-block", width: "10%", height: "60%" }} >
                      <button onClick={updateDoc}>Update Document</button>
                      <button onClick={deleteDoc}>Delete Document</button>
                      <button onClick={metaDoc}>Document Metadata</button>
                  </div>
              </div>

              <form onSubmit={uploadDoc}>
                  <div style={{ display: "inline-block", width: "10%", height: "20%" }} >
                      <b>Upload New Document: </b>
                  </div>
                  <div style={{ display: "inline-block", width: "70%", height: "20%" }} >
                      <input type="text" id="NewDocName" />
                      <pre style={{ display: "inline-block" }} >     </pre>
                      <input type="file" id="NewDocFile"/>
                  </div>
                  <div style={{ display: "inline-block", width: "10%", height: "20%" }} >
                      <input type="submit"/>
                  </div>
              </form>

          </div>
    </>
  )
}

export default App*/
import { useEffect, useState } from 'react';
import './App.css';

function App() {
    const [documents, setDocuments] = useState([]);
    const [selectedDoc, setSelectedDoc] = useState(null);
    const [newDocName, setNewDocName] = useState("");
    const [newDocFile, setNewDocFile] = useState(null);

    useEffect(() => {
        fetch("http://localhost:8080/api/documents")
            .then(res => res.json())
            .then(data => {
                console.log("API response:", data);
                if (Array.isArray(data)) {
                    setDocuments(data);
                } else {
                    console.error("Unerwartete Antwort:", data);
                    setDocuments([]);
                }
            })
            .catch(err => {
                console.error("Fehler beim Laden:", err);
                setDocuments([]);
            });
    }, []);


    function changeCurrentDoc(id) {
        fetch(`http://localhost:8080/api/documents/${id}`)
            .then(res => res.json())
            .then(data => setSelectedDoc(data))
            .catch(err => console.error("Fehler beim Laden des Dokuments:", err));
    }

    function updateDoc() {
        if (!selectedDoc) return;
        fetch(`http://localhost:8080/api/documents/${selectedDoc.id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(selectedDoc)
        }).then(() => console.log("Dokument aktualisiert"));
    }

    function deleteDoc() {
        if (!selectedDoc) return;
        fetch(`http://localhost:8080/api/documents/${selectedDoc.id}`, {
            method: "DELETE"
        }).then(() => console.log("Dokument gelöscht"));
    }

    function metaDoc() {
        if (!selectedDoc) return;
        fetch(`http://localhost:8080/api/documents/${selectedDoc.id}/data`)
            .then(res => res.json())
            .then(data => console.log("Metadaten:", data));
    }

    function uploadDoc(e) {
        e.preventDefault();
        const formData = new FormData();
        formData.append("name", newDocName);
        formData.append("file", newDocFile);

        fetch("http://localhost:8080/api/documents", {
            method: "POST",
            body: formData
        }).then(() => console.log("Dokument hochgeladen"));
    }

    return (
        <>
            <h1>Document Management System</h1>
            <div style={{ display: "flex", gap: "2rem" }}>
                <div style={{ width: "20%" }}>
                    <h3>Dokumente</h3>
                    {documents.map(doc => (
                        <button key={doc.id} onClick={() => changeCurrentDoc(doc.id)}>
                            {doc.title || "Dokument"}
                        </button>
                    ))}
                </div>
                <div style={{ width: "60%" }}>
                    <h3>Aktuelles Dokument</h3>
                    {selectedDoc ? (
                        <>
                            <h2>{selectedDoc.title}</h2>
                            <p>{selectedDoc.summary}</p>
                        </>
                    ) : (
                        <p>Kein Dokument ausgewählt</p>
                    )}
                </div>
                <div style={{ width: "20%" }}>
                    <button onClick={updateDoc}>Update</button>
                    <button onClick={deleteDoc}>Delete</button>
                    <button onClick={metaDoc}>Metadata</button>
                </div>
            </div>

            <form onSubmit={uploadDoc} style={{ marginTop: "2rem" }}>
                <h3>Neues Dokument hochladen</h3>
                <input
                    type="text"
                    placeholder="Dokumentname"
                    value={newDocName}
                    onChange={e => setNewDocName(e.target.value)}
                />
                <input
                    type="file"
                    onChange={e => setNewDocFile(e.target.files[0])}
                />
                <button type="submit">Hochladen</button>
            </form>
        </>
    );
}

export default App;

