from typing import List, Dict
from sentence_transformers import SentenceTransformer, util
import torch


class ProjectMatcher:

    def __init__(self):
        self.model = SentenceTransformer("sentence-transformers/all-MiniLM-L6-v2")

    def match(self, candidate_json, projects_json, mode: str = "all"):

        candidate_skills = self._prepare_skills(candidate_json["skills"])

        results = []

        for project in projects_json:
            project_skills = self._prepare_skills(project["skills"])

            score, reason = self._calculate_score(candidate_skills, project_skills)

            results.append({
                "project_id": project["id"],
                "project_name": project["name"],
                "score": round(score * 100, 2),
                "reason": reason,
            })

        results = sorted(results, key=lambda x: x["score"], reverse=True)

        if mode == "best":
            return results[0] if results else None

        return results

    def _prepare_skills(self, skills: List[Dict]):
        prepared = []

        for s in skills:
            name = self._normalize_name(s.get("name"))

            if not name:
                continue

            prepared.append({
                "name": name,
                "years": s.get("years") or 0,
                "level": s.get("level")
            })

        return prepared

    def _normalize_name(self, name: str):
        return name.lower().strip()

    def _calculate_score(self, candidate_skills, project_skills):
        if not candidate_skills or not project_skills:
            return 0.0, []

        cand_names = [s["name"] for s in candidate_skills]
        proj_names = [s["name"] for s in project_skills]

        cand_emb = self.model.encode(cand_names, convert_to_tensor=True)
        proj_emb = self.model.encode(proj_names, convert_to_tensor=True)
        sim_matrix = util.cos_sim(proj_emb, cand_emb)

        weighted_scores = []
        total_weight = 0
        missing_critical_skills = []

        for i, p_skill in enumerate(project_skills):
            weight = p_skill.get("weight", 1.0)
            best_match_idx = torch.argmax(sim_matrix[i]).item()
            sim = sim_matrix[i][best_match_idx].item()

            is_missing = sim < 0.65

            if is_missing and weight >= 1.0:
                missing_critical_skills.append(p_skill["name"])
                weighted_scores.append(0)
            elif is_missing:
                weighted_scores.append(0)
            else:
                adjusted_sim = min(1.0, sim / 0.9)
                req_exp = p_skill.get("required_years", p_skill.get("years", 0))
                actual_exp = candidate_skills[best_match_idx].get("years", 0)

                exp_factor = self._calc_single_exp(actual_exp, req_exp)
                skill_score = adjusted_sim * (0.8 + 0.2 * exp_factor)
                weighted_scores.append(min(1.0, skill_score) * weight)

            total_weight += weight

        base_score = sum(weighted_scores) / total_weight if total_weight > 0 else 0

        final_score = base_score
        for _ in missing_critical_skills:
            final_score *= 0.7

        reasons = [f"Отсутствует критический навык: {s}" for s in missing_critical_skills]

        return float(final_score), reasons

    def _calc_single_exp(self, actual, required):
        if required <= 0: return 1.0
        return min(actual / required, 1.2)

    def _bert_similarity(self, candidate, project):

        candidate_names = [s["name"] for s in candidate]
        project_names = [s["name"] for s in project]

        if not candidate_names or not project_names:
            return 0.0

        cand_emb = self.model.encode(candidate_names, convert_to_tensor=True)
        proj_emb = self.model.encode(project_names, convert_to_tensor=True)

        sim_matrix = util.cos_sim(proj_emb, cand_emb)

        max_sim_per_project_skill = sim_matrix.max(dim=1).values

        score = max_sim_per_project_skill.mean().item()

        return float(score)

    def _experience_score(self, candidate, project):
        total = 0
        count = 0

        candidate_map = {s["name"]: s for s in candidate}

        for ps in project:
            name = ps["name"]

            if name not in candidate_map:
                continue

            required = ps.get("years", 0)
            actual = candidate_map[name].get("years", 0)

            if required == 0:
                total += 1
            else:
                total += min(actual / required, 1)

            count += 1

        return total / count if count else 0

    def _level_score(self, candidate, project):
        level_map = {
            "junior": 1,
            "middle": 2,
            "senior": 3
        }

        total = 0
        count = 0

        candidate_map = {s["name"]: s for s in candidate}

        for ps in project:
            name = ps["name"]

            if name not in candidate_map:
                continue

            required = level_map.get((ps.get("level") or "").lower(), 0)
            actual = level_map.get((candidate_map[name].get("level") or "").lower(), 0)

            if required == 0:
                total += 1
            else:
                total += min(actual / required, 1)

            count += 1

        return total / count if count else 0