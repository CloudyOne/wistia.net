﻿#region License, Terms and Conditions

//
// ProjectService.cs
//
// Author: Kori Francis <twitter.com/djbyter>
// Copyright (C) 2014 Kori Francis. All rights reserved.
// 
// THIS FILE IS LICENSED UNDER THE MIT LICENSE AS OUTLINED IMMEDIATELY BELOW:
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
#endregion

using System.Collections.Generic;
using System.Threading.Tasks;
using Wistia.Core.Services.Data.Models;

namespace Wistia.Core.Services.Data
{
    public class ProjectService : WistiaClientBase, IWistiaApiEndpoint<Project>
    {
        public ProjectService(string apiKey) : base(apiKey)
        {
        }

        public async Task<List<Project>> All()
        {
            var allProject = new List<Project>();
            var lastPull = false;
            var page = 0;
            while (!lastPull)
            {
                var tempmedia = await GetRequest<List<Project>>($"projects.json?page={page}");
                allProject.AddRange(tempmedia);
                page++;
                if (tempmedia.Count < 100)
                    lastPull = true;
            }
            return allProject;
        }
  
        public async Task<Project> GetById(string hashedProjectId)
        {
            return await GetRequest<Project>("projects/{0}.json", hashedProjectId);
        }

        /// <summary>
        /// Create a new project.
        /// </summary>
        /// <param name="project">The project to create</param>
        /// <returns>The created project</returns>
        public async Task<Project> Create(Project project)
        {
            return await PostRequest<Project, Project>(project, "projects.json");
        }
  
        /// <summary>
        /// Update settings for an existing project under your control (ie: only the ones you own).
        /// </summary>
        /// <param name="project">The project to update</param>
        public async Task Update(Project project)
        {
            await PutRequest<Project, Project>(project, "projects/{0}.json", project.hashedId);
        }
  
        /// <summary>
        /// Delete method for projects
        /// </summary>
        /// <param name="projectId">The projectId to delete</param>
        public async Task Delete(string projectId)
        {
            await DeleteRequest("/projects/{0}", projectId);
        }
    }
}
