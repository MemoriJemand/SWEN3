//import { useState } from 'react'
import './App.css'

// install react router for navigation
// npm install react-router-dom

function App() {
    /*    const [count, setCount] = useState(0)*/

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

export default App
