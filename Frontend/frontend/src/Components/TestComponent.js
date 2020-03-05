import React from 'react';

export default class TestComponent extends React.Component {
    constructor () {
        super();

        this.state = {
            testItems: undefined
        }

        this.loadItems();
    }

    async loadItems() {
        let response = await fetch('https://localhost:44394/test');
        if (!response.ok) {
            console.error(response);
        }
        
        this.setState({ testItems: await response.json() });
    }

    render () {
        return (
            <span>
                {
                    this.state.testItems ?
                        this.state.testItems.length + " Items Retrieved From Backend"
                    :
                        "Retrieving items from backend..."
                }
            </span>
        )
    }
}