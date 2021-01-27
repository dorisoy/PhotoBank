import '../modules/styles.css'
import React from 'react'
import { Route, /*IndexRoute,*/ Redirect } from 'react-router'
import { ServerRoute } from 'react-project'
import hello from './api/hello'
//import App from './components/App'
import Auth from './components/Auth'
import Photos from './components/Photos'
//import Home from './components/Home'
import NoMatch from './components/NoMatch'
//import Dragon from './components/Dragon'

export default (
    <Route>
        <Route path="/" component={Auth} />
        <Route path="/auth" component={Auth} />
        <Route path="/photos" component={Photos} />
        <ServerRoute path="/api">
            <ServerRoute path=":hello" get={hello} />
        </ServerRoute>
        <Redirect from="/not-dragon" to="/dragon" />
        <Route path="*" status={404} component={NoMatch} />
    </Route>
)
