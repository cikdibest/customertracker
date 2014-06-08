

customerApp.factory('solutionFactory', function ($http) {
    return {
        getSolutions: function (pageNumber, pageSize, sortBy, sortDir) {
            return $http.get(solutionApiUrl.getsolutions, { params: { pageNumber: pageNumber, pageSize: pageSize, sortBy: sortBy, sortDir: sortDir } });
        },
        addSolution: function (solution) {
            return $http.post(solutionApiUrl.postsolution, solution);
        },
        deleteSolution: function (solution) {
            return $http.delete(solutionApiUrl.deletesolution + solution.Id);
        },
        updateSolution: function (solution) {
            return $http.put(solutionApiUrl.putsolution + solution.Id, solution);
        }

    };
});